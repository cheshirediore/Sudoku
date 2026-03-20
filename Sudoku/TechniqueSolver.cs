using System;
using System.Collections.Generic;

namespace Sudoku;

public class TechniqueSolver(Puzzle sudokuGrid) : Solver
{
    private delegate HashSet<Action> Technique();
    private Technique? lastTechnique = null;

    private int failStreak = 0; // Keeps track of consecutive technique failures
    private const int MAX_SOLVE_ITERATIONS = Int32.MaxValue;
    private const int MAX_FAIL_STREAK = 30;

    #region SolverInterface
    /// <summary>
    /// The puzzle to be solved. It is updated with the solution.
    /// </summary>
    /// <remarks>
    /// Overrides the property for the Solver abstract class.
    /// </remarks>
    public override Puzzle SudokuPuzzle { get; init; } = (Puzzle)sudokuGrid.Clone();
    /// <summary>
    /// The maximum number of solutions to search for. Once this number is reached, 
    /// it will return the results without looking further.
    /// </summary>
    /// <remarks>
    /// Overrides the property for the Solver abstract class.
    /// </remarks>
    public override int MaxSolutions { get; set; } = -1;


    public override List<Puzzle> Solve()
    {
        List<Puzzle> solutions = [];

        Technique? technique = GetNextTechnique();

        // Iteration counter to prevent infinite loops
        int iterations_completed = 0;
        while (!SudokuPuzzle.IsComplete()) // Loop until all cells are filled
        {
            // Try to apply the technique
            bool techniqueSuccess = ApplyTechnique(technique);
            
            if (techniqueSuccess) // On success, reset the fail streak counter
            {
                failStreak = 0;
            } 
            else // If the technique failed, move on to the next one
            {
                failStreak++;
                technique = GetNextTechnique();
                break; // debug
            }

            // DEBUG
            Console.WriteLine(SudokuPuzzle);
            Console.WriteLine();

            // Give up if it's iterated through the techniques several times and still hasn't solved it.
            if (failStreak > MAX_FAIL_STREAK)
            {
                Console.WriteLine($"[TechniqueSolver.Solve()] Failed {failStreak} times in a row. Giving up.");
                break;
            }

            // Infinite loop failsafe check
            iterations_completed++;
            if (iterations_completed > MAX_SOLVE_ITERATIONS) break;
        }

        // Verify that the solution is valid, and only return it if it is.
        if (SudokuPuzzle.IsComplete() || SudokuPuzzle.IsConsistent())
        {
            solutions.Add(SudokuPuzzle); 
        }
        return solutions;
    }

    #endregion


    

    /// <summary>
    /// Get the next rule to apply, based on the previous rule.
    /// </summary>
    private Technique? GetNextTechnique()
    {
        // TODO: replace this with a more efficient (or at least cleaner) looping mechanic, like a list to loop over

        // First, look for Naked Single candidates
        if (lastTechnique == null)
        {
            return NakedSingle;
        }
        // Next, look for Hidden Single candidates
        if (lastTechnique == NakedSingle)
        {
            return HiddenSingle;
        }
        // Next, look for Naken Pair candidates
        if (lastTechnique == HiddenSingle)
        {
            return NakedPair;
        }
        // Next, look for Hidden Pair candidates
        // if (lastTechnique == NakedPair)
        // {
        //     return HiddenPair;
        // }
        // ...
        // TODO: Add the rest of the rule orders
        
        // Returning null after all other techniques causes it to loop back to NakedSingle
        return null;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="technique"></param>
    /// <returns>
    /// False if any of the following occurs:
    ///     - Technique is null, or
    ///     - Technique does not return any actions, or
    ///     - Any action returned by the technique failed
    /// True iff:
    ///     - A non-zero amount of actions was returned by the technique
    ///     - All actions returned by the technique executed successfully
    /// </returns>
    /// <exception cref="InvalidOperationException"></exception>
    private bool ApplyTechnique(Technique? technique)
    {
        // We want to updarte the lastTechnique even if this technique is null.
        lastTechnique = technique;

        if (technique is null) return false;

        HashSet<Action> actions = technique();

        if (actions.Count == 0) return false;

        // return value is the conjunction of the results of resolving each action; i.e. true iff all return true
        bool success = true;

        foreach (var action in actions)
        {
            switch (action.Type)
            {
                case ActionType.SET:
                    success = success && SudokuPuzzle.SetCellValue(action.CellIndex, action.CellValue);
                    break;
                case ActionType.REMOVE:
                    success = success && SudokuPuzzle.RemoveCellCandidate(action.CellIndex, action.CellValue);
                    break;
                default:
                    throw new InvalidOperationException(); // TODO: Find a better exception to throw here
            }
        }
        return success;
    }

    #region Easy
    /// <summary>
    /// Identify Naked Single candidates, if they exists.
    /// </summary>
    /// <returns>
    /// A list of integer arrays of the format [cellIndex, cellValue], where cellValue is what the
    /// cell should be set to, and cellIndex is the index of the cell.
    /// 
    /// If no such candidates exist, returns an empty list.
    /// </returns>
    /// <remarks>
    /// NakedSingle is unique in that it is not region-specific. A single cell can be considered
    /// in isolation to determine whether it qualifies.
    /// 
    /// A Naked Single is when only one possible candidate exists for a given cell.
    /// </remarks>
    internal HashSet<Action> NakedSingle()
    {
        // Initialize the empty list for the return
        HashSet<Action> results = [];
        // For each cell, identify any Naked Single candidates.
        for (int cellIndex = 0; cellIndex < Grid.SIZE; cellIndex++)
        {
            Cell cell = SudokuPuzzle.CellGrid.GetVertex(cellIndex);
            if (cell.Candidates.Count == 1)
            {
                results.Add(new Action(ActionType.SET, cellIndex, cell.Candidates[0]));
            }
        }
        return results;
    }

    /// <summary>
    /// Identify Hidden Single candidates, if they exists.
    /// </summary>
    /// <returns>
    /// A list of integer arrays of the format [cellIndex, cellValue], where cellValue is what the
    /// cell should be set to, and cellIndex is the index of the cell.
    /// 
    /// If no such candidates exist, returns an empty list.
    /// </returns>
    internal HashSet<Action> HiddenSingle()
    {
        // Initialize the empty list for the return
        HashSet<Action> results = [];
        
        // Search the Blocks
        // For each of the block
        foreach (Region region in SudokuPuzzle.Regions[RegionType.BLOCK])
        {
            // results.AddRange(FindHiddenSinglesInRegion(region));
            foreach (var item in FindHiddenSinglesInRegion(region))
            {
                results.Add(item);
            }
        }

        // Search the Columns
        foreach (Region region in SudokuPuzzle.Regions[RegionType.COLUMN])
        {
            // results.AddRange(FindHiddenSinglesInRegion(region));
            foreach (var item in FindHiddenSinglesInRegion(region))
            {
                results.Add(item);
            }
        }

        // Search the Rows
        foreach (Region region in SudokuPuzzle.Regions[RegionType.ROW])
        {
            // results.AddRange(FindHiddenSinglesInRegion(region));
            foreach (var item in FindHiddenSinglesInRegion(region))
            {
                results.Add(item);
            }
        }
        return results;
    }
    
    private static HashSet<Action> FindHiddenSinglesInRegion(Region region)
    {
        HashSet<Action> results = [];

        // Create and initialize the dictionary with keys 1 through 9 (inclusive)
        Dictionary<int, List<Cell>> candidateCellMap = [];
        for (int candidateValue = 1; candidateValue < 10; candidateValue++)
        {
            candidateCellMap[candidateValue] = [];
        }

        // Check each cell
        foreach (Cell cell in region.Cells)
        {
            // Skip cells that already have a set value
            if (cell.Value != 0)
            {
                continue;
            }
            // For each candidate that cell could be, add the cell to the map for that candidate
            foreach (int candidate in cell.Candidates)
            {
                candidateCellMap[candidate].Add(cell);
            }
        }
        // Check which candidates only mapped to one cell. Those are the hidden singles.
        foreach (int candidate in candidateCellMap.Keys)
        {
            if (candidateCellMap[candidate].Count == 1)
            {
                // Add the cell index and candidate to the result list, unless it's already in the list
                Cell cell = candidateCellMap[candidate][0];
                Action action = new Action(ActionType.SET, cell.Index, candidate);
                results.Add(action);
            }
        }

        return results;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="region"></param>
    /// <returns>
    /// A list of cell arrays, each of which contains the a pair of cells consistuting a naked pair.
    /// </returns>
    internal List<Cell[]> FindNakedPairCellsInRegion(Region region)
    {
        List<Cell[]> nakedPairs = [];
        // Find all cells that have only two candidates
        List<Cell> pairCandidates = [];
        foreach (Cell cell in region.Cells)
        {
            if (cell.Candidates.Count == 2)
            {
                pairCandidates.Add(cell);
            }
        }

        // Compare each cell to each other cell. If they have the same two candidates, pair them off.
        // TODO: rewrite this more efficiently to not double every pair
        foreach (Cell cell in pairCandidates)
        {
            foreach (Cell other in pairCandidates)
            {
                if (cell == other)
                {
                    continue;
                }
                // Candidates is assumed to be sorted
                if (cell.Candidates[0] == other.Candidates[0] && cell.Candidates[1] == other.Candidates[1])
                {
                    nakedPairs.Add([cell, other]);
                }
            }
        }

        return nakedPairs;
    }

    /// <summary>
    /// Identify Naked Pairs of candidates, if they exists.
    /// </summary>
    /// <returns>
    /// 
    /// </returns>
    internal HashSet<Action> NakedPair()
    {
        // Initialize the empty list for the return
        HashSet<Action> results = [];
        // Search the Blocks
        foreach (Region region in SudokuPuzzle.Regions[RegionType.BLOCK])
        {
            List<Cell[]> nakedPairs = FindNakedPairCellsInRegion(region);
            foreach (Cell[] pair in nakedPairs)
            {
                foreach (Cell cell in region.Cells)
                {
                    // Skip clue cells
                    if (cell.IsClue) continue;

                    // If the cell isn't one of the naked pair cells, remove those candidates from the cell
                    if (cell.Index != pair[0].Index && cell.Index != pair[1].Index)
                    {
                        // The naked pair share the same candidates by definition, so we just check one
                        foreach (int candidate in pair[0].Candidates)
                        {
                            Action action = new(ActionType.REMOVE, cell.Index, candidate);
                            results.Add(action);
                        }
                    }
                }
            }
        }

        // Search the Columns
        foreach (Region region in SudokuPuzzle.Regions[RegionType.COLUMN])
        {
            List<Cell[]> nakedPairs = FindNakedPairCellsInRegion(region);
            foreach (Cell[] pair in nakedPairs)
            {
                foreach (Cell cell in region.Cells)
                {
                    // Skip clue cells
                    if (cell.IsClue) continue;
                    
                    // If the cell isn't one of the naked pair cells, remove those candidates from the cell
                    if (cell.Index != pair[0].Index && cell.Index != pair[1].Index)
                    {
                        // The naked pair share the same candidates by definition, so we just check one
                        foreach (int candidate in pair[0].Candidates)
                        {
                            Action action = new(ActionType.REMOVE, cell.Index, candidate);
                            results.Add(action);
                            
                        }
                    }
                }
            }
        }

        // Search the Rows
        foreach (Region region in SudokuPuzzle.Regions[RegionType.ROW])
        {
            List<Cell[]> nakedPairs = FindNakedPairCellsInRegion(region);
            foreach (Cell[] pair in nakedPairs)
            {
                foreach (Cell cell in region.Cells)
                {
                    // Skip clue cells
                    if (cell.IsClue) continue;
                    
                    // If the cell isn't one of the naked pair cells, remove those candidates from the cell
                    if (cell.Index != pair[0].Index && cell.Index != pair[1].Index)
                    {
                        // The naked pair share the same candidates by definition, so we just check one
                        foreach (int candidate in pair[0].Candidates)
                        {
                            Action action = new(ActionType.REMOVE, cell.Index, candidate);
                                results.Add(action);
                        }
                    }
                }
            }
        }

        return results;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="region"></param>
    /// <returns>
    /// A list of cell arrays, each of which contains the a pair of cells consistuting a hidden pair.
    /// </returns>
    internal List<Cell[]> FindHiddenPairCellsInRegion(Region region)
    {
        List<Cell[]> hiddenPairs = [];
        return hiddenPairs;
    }


    /// <summary>
    /// Identify Hidden Pairs of candidates, if they exists.
    /// </summary>
    /// <returns>
    /// 
    /// </returns>
    internal HashSet<Action> HiddenPair()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // Search the Blocks
        // Search the Columns
        // Search the Rows
        // return results;
    }

    #endregion

    #region Medium
    /// <summary>
    /// 
    /// </summary>
    /// <param name="region"></param>
    /// <returns>
    /// A list of cell arrays, each of which contains the a pair of cells consistuting a naked pair.
    /// </returns>
    internal List<Cell[]> FindNakedTripleCellsInRegion(Region region)
    {
        List<Cell[]> nakedTriples = [];
        // Find all cells that have only two candidates
        List<Cell> tripleCandidates = [];
        foreach (Cell cell in region.Cells)
        {
            if (cell.Candidates.Count == 3)
            {
                tripleCandidates.Add(cell);
            }
        }

        // Compare each cell to each other cell. If they have the same two candidates, pair them off.
        // In theory, a cell can be paired with more than one cell, but this would only happen in an invalid puzzle.
        // This will, however, double every pair.
        foreach (Cell cell in tripleCandidates)
        {
            foreach (Cell other in tripleCandidates)
            {
                if (cell == other)
                {
                    continue;
                }
                // Candidates is assumed to be sorted
                if (cell.Candidates[0] == other.Candidates[0] && cell.Candidates[1] == other.Candidates[1])
                {
                    nakedTriples.Add([cell, other]);
                }
            }
        }

        return nakedTriples;
    }
    internal HashSet<Action> NakedTriple()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // return results;
    }

    internal HashSet<Action> HiddenTriple()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // return results;
    }

    internal HashSet<Action> PointingPair()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // return results;
    }

    internal HashSet<Action> BoxLineReduction()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // return results;
    }
    #endregion

    #region Hard
    internal HashSet<Action> XWing()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // return results;
    }

    internal HashSet<Action> Swordfish()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // return results;
    }

    internal HashSet<Action> YWing()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // return results;
    }
    #endregion

    #region VeryHard
    internal HashSet<Action> Jellyfish()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // return results;
    }

    internal HashSet<Action> XYZWing()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // return results;
    }

    internal HashSet<Action> Coloring()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // return results;
    }
    #endregion

    #region Expert
    internal HashSet<Action> ForcingChain()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // return results;
    }

    internal HashSet<Action> Backtracking()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // HashSet<Action> results = [];
        // return results;
    }
    #endregion
}