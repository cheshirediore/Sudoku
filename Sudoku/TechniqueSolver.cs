using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;

namespace Sudoku;

public class TechniqueSolver(Puzzle sudokuGrid) : Solver
{
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

    private const int MAX_SOLVE_ITERATIONS = 256;

    public override List<Puzzle> Solve()
    {
        List<Puzzle> solutions = [];
        
        // TODO: loop through applying rules to the current board state to determine
        //       the next valid move. Terminate and hand off to the Backtracker if 
        //       no valid moves are determined by applying rules.
        
        Technique? technique = GetNextTechnique(null);
        if (technique is null)
        {
            return solutions;
        }
        
        List<Action> results = technique();

        // Debug Iteration Count
        int interation_count = 0;
        while (results.Count > 0)
        {
            Console.WriteLine($"Iteration #{interation_count}");
            Console.WriteLine(SudokuPuzzle);
            foreach (var result in results)
            {
                // Console.WriteLine($"Executing Action {result}");
                // Techniques that set values:
                if (result.Type == ActionType.SET)
                {
                    SudokuPuzzle.SetCellValue(result.CellIndex, result.CellValue);
                }
                // Techniques that remove candidates:
                else if (result.Type == ActionType.REMOVE)
                {
                    SudokuPuzzle.RemoveCellCandidate(result.CellIndex, result.CellValue);
                }
            }

            technique = GetNextTechnique(technique);
            if (technique is null) continue;
            results = technique();

            // Terminate loop if it ran too long
            interation_count++;
            if (interation_count > MAX_SOLVE_ITERATIONS) break;
        }
        //// Disabled for testing
        // // Verify that the solution is valid, and only return it if it is.
        // if (SudokuPuzzle.IsComplete() || SudokuPuzzle.IsConsistent())
        // {
            solutions.Add(SudokuPuzzle); 

        // }
        return solutions;
    }

    #endregion


    // TODO: Create a class to handle the technique results, and use that as the return value instead of a List
    //       Additional nuance is needed to handle different technique types, but we still want to use one delegate
    //       as the entry point.
    private delegate List<Action> Technique();

    /// <summary>
    /// Get the next rule to apply, based on the previous rule.
    /// </summary>
    private Technique? GetNextTechnique(Technique? lastTechnique)
    {
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
        // Next, look for Hidden Pair candidates
        // ...
        // TODO: Add the rest of the rule orders
        return null;
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
    internal List<Action> NakedSingle()
    {
        Console.WriteLine("NakedSingle()");
        // Initialize the empty list for the return
        List<Action> results = [];
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
    internal List<Action> HiddenSingle()
    {
        Console.WriteLine("HiddenSingle()");
        // Initialize the empty list for the return
        List<Action> results = [];
        
        // Search the Blocks
        // For each of the block
        foreach (Region region in SudokuPuzzle.Regions[RegionType.BLOCK])
        {
            // results.AddRange(FindHiddenSinglesInRegion(region));
            foreach (var item in FindHiddenSinglesInRegion(region))
            {
                if (!results.Contains(item))
                {
                    results.Add(item);
                }
            }
        }

        // Search the Columns
        foreach (Region region in SudokuPuzzle.Regions[RegionType.COLUMN])
        {
            // results.AddRange(FindHiddenSinglesInRegion(region));
            foreach (var item in FindHiddenSinglesInRegion(region))
            {
                if (!results.Contains(item))
                {
                    results.Add(item);
                }
            }
        }

        // Search the Rows
        foreach (Region region in SudokuPuzzle.Regions[RegionType.ROW])
        {
            // results.AddRange(FindHiddenSinglesInRegion(region));
            foreach (var item in FindHiddenSinglesInRegion(region))
            {
                if (!results.Contains(item))
                {
                    results.Add(item);
                }
            }
        }
        return results;
    }
    
    private static List<Action> FindHiddenSinglesInRegion(Region region)
    {
        List<Action> results = [];

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
    internal List<Action> NakedPair()
    {
        Console.WriteLine("NakedPair()");
        // Initialize the empty list for the return
        List<Action> results = [];
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
                            if (!results.Contains(action))
                            {
                                results.Add(action);
                            }
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
                            if (!results.Contains(action))
                            {
                                results.Add(action);
                            }
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
                            if (!results.Contains(action))
                            {
                                results.Add(action);
                            }
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
    internal List<Action> HiddenPair()
    {
        Console.WriteLine("HiddenPair()");
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
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
    internal List<Action> NakedTriple()
    {
        Console.WriteLine("NakedTriple()");
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
        // return results;
    }

    internal List<Action> HiddenTriple()
    {
        Console.WriteLine("HiddenTriple()");
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
        // return results;
    }

    internal List<Action> PointingPair()
    {
        Console.WriteLine("PointingPair()");
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
        // return results;
    }

    internal List<Action> BoxLineReduction()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
        // return results;
    }
    #endregion

    #region Hard
    internal List<Action> XWing()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
        // return results;
    }

    internal List<Action> Swordfish()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
        // return results;
    }

    internal List<Action> YWing()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
        // return results;
    }
    #endregion

    #region VeryHard
    internal List<Action> Jellyfish()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
        // return results;
    }

    internal List<Action> XYZWing()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
        // return results;
    }

    internal List<Action> Coloring()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
        // return results;
    }
    #endregion

    #region Expert
    internal List<Action> ForcingChain()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
        // return results;
    }

    internal List<Action> Backtracking()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<Action> results = [];
        // return results;
    }
    #endregion
}