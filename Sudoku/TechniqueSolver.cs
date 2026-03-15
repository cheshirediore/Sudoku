using System;
using System.Collections.Generic;
using Sudoku.Technique;

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

    public override List<Puzzle> Solve()
    {
        List<Puzzle> solutions = [];
        // Technique-based solvers always result in a solution as long as the puzzle is valid.
        solutions.Add(SudokuPuzzle); 
        // TODO: loop through applying rules to the current board state to determine
        //       the next valid move. Terminate and hand off to the Backtracker if 
        //       no valid moves are determined by applying rules.
        
        Technique? technique = GetNextTechnique(null);
        if (technique != null)
        {
            List<Result> results = technique();

            foreach (var result in results)
            {
                // Techniques that set values:
                if (result.Type == ResultType.SET)
                {
                    SudokuPuzzle.SetCellValue(result.CellIndex, result.CellValue);
                }
                // Techniques that remove candidates:
                else if (result.Type == ResultType.REMOVE)
                {
                    SudokuPuzzle.RemoveCellCandidate(result.CellIndex, result.CellValue);
                }
            }
        }
        //// Disabled for testing
        // // Verify that the solution is valid, and only return it if it is.
        // if (!SudokuPuzzle.IsComplete() || !SudokuPuzzle.IsConsistent())
        // {
        //     solutions.Clear();
        // }
        return solutions;
    }

    #endregion


    // TODO: Create a class to handle the technique results, and use that as the return value instead of a List
    //       Additional nuance is needed to handle different technique types, but we still want to use one delegate
    //       as the entry point.
    private delegate List<Result> Technique();

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
    
    #region Techniques
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
    internal List<Result> NakedSingle()
    {
        // Initialize the empty list for the return
        List<Result> results = [];
        // For each cell, identify any Naked Single candidates.
        for (int cellIndex = 0; cellIndex < Grid.SIZE; cellIndex++)
        {
            Cell cell = SudokuPuzzle.CellGrid.GetVertex(cellIndex);
            if (cell.Candidates.Count == 1)
            {
                results.Add(new Result(ResultType.SET, cellIndex, cell.Candidates[0]));
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
    internal List<Result> HiddenSingle()
    {
        // Initialize the empty list for the return
        List<Result> results = [];
        
        // Search the Blocks
        // For each of the block
        foreach (Region region in SudokuPuzzle.Regions[RegionType.BLOCK])
        {
            results.AddRange(FindHiddenSinglesInRegion(region));
        }

        // Search the Columns
        foreach (Region region in SudokuPuzzle.Regions[RegionType.COLUMN])
        {
            results.AddRange(FindHiddenSinglesInRegion(region));
        }

        // Search the Rows
        foreach (Region region in SudokuPuzzle.Regions[RegionType.ROW])
        {
            results.AddRange(FindHiddenSinglesInRegion(region));
        }
        return results;
    }
    
    private static List<Result> FindHiddenSinglesInRegion(Region region)
    {
        List<Result> results = [];

        // Create and initialize the dictionary with keys 1 through 9 (inclusive)
        Dictionary<int, List<Cell>> candidateCellMap = [];
        for (int candidateValue = 1; candidateValue < 10; candidateValue++)
        {
            candidateCellMap[candidateValue] = [];
        }

        // Check each cell
        foreach (Cell cell in region.Cells)
        {
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
                // Add the cell index and candidate to the result list. Three steps are used for readability.
                Cell cell = candidateCellMap[candidate][0];
                results.Add(new Result(ResultType.SET, cell.Index, candidate));
            }
        }

        return results;
    }

    /// <summary>
    /// Identify Naked Pairs of candidates, if they exists.
    /// </summary>
    /// <returns>
    /// 
    /// </returns>
    internal List<int[]> NakedPair()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<int[]> results = [];
        // Search the Blocks
        // Search the Columns
        // Search the Rows
        // return results;
    }

    /// <summary>
    /// Identify Hidden Pairs of candidates, if they exists.
    /// </summary>
    /// <returns>
    /// 
    /// </returns>
    internal List<int[]> HiddenPair()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        // List<int[]> results = [];
        // Search the Blocks
        // Search the Columns
        // Search the Rows
        // return results;
    }
    #endregion
}