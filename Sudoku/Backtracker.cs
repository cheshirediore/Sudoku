using System.Collections.Generic;

namespace Sudoku;

public class Backtracker: Solver
{
    public override int MaxSolutions { get; set; } = -1;
    public override Grid Puzzle { get; init; }

    public Backtracker(Grid sudokuGrid)
    {
        Puzzle = sudokuGrid;
    }

    public override List<Grid> Solve()
    {
        return Backtrack(Puzzle, Puzzle, new List<Grid>());
    }

    /*
     * From wikipedia:
     * P is the data
     * c is a partial candidate
     *
     * backtrack(P, c)
     * procedure backtrack(P, c) is
     * if reject(P, c) then return
     * if accept(P, c) then output(P, c)
     * s ← first(P, c)
     * while s ≠ NULL do
     *     backtrack(P, s)
     *     s ← next(P, s)
     */

    public List<Grid> Backtrack(Grid data, Grid candidate, List<Grid> solutions)
    {
        // Stop looking if more than one solution has been found. We only care about valid sudoku puzzles.
        if (MaxSolutions > 0 && solutions.Count >= MaxSolutions)
        {
            return solutions;
        }
        if (Reject(candidate))
        {
            return solutions;
        }
        if (Accept(candidate))
        {
            return Output(candidate, solutions);
        }

        Grid? nextCandidate = First(data, candidate);
        while (nextCandidate != null)
        {
            solutions = Backtrack(candidate, nextCandidate, solutions);
            nextCandidate = Next(candidate, nextCandidate);
        }
        return solutions;
    }

    // root(P): return the partial candidate at the root of the search tree
    public Grid Root()
    {
        return Puzzle;
    }

    // output(P, c): use the solution c of P, as appropriate to the application
    public static List<Grid> Output(Grid candidate, List<Grid> solutions)
    {
        solutions.Add(candidate);
        return solutions;
    }

    #region Validation
    // reject(P, c): return true only if the partial candidate c is not worth completing
    public static bool Reject(Grid candidate)
    {
        // Only check the last updated cell's row, column, and block.
        return !candidate.IsLastUpdateValid(); 
        /*
        for (int i = 0; i < 9; i++)
        {
            // Check columns
            var column = candidate.GetColumn(i);
            // var column = GetColumnValues(candidate, i);
            HashSet<int> values = new();
            
            for (int index = 0; index < column.Length; index++)
            {
                if (column[index] != 0 && !values.Add(System.Math.Abs(column[index])))
                {
                    return true;
                }
            }
            values.Clear();

            // Check rows
            // var row = GetRowValues(candidate, i);
            var row = candidate.GetRow(i);
            for (int index = 0; index < row.Length; index++)
            {
                if (row[index] != 0 && !values.Add(System.Math.Abs(row[index])))
                {
                    return true;
                }
            }
            values.Clear();


            // Check blocks
            // var block = GetBlockValues(candidate, i);
            var block = candidate.GetBlock(i);
            for (int index = 0; index < block.Length; index++)
            {
                if (block[index] != 0 && !values.Add(System.Math.Abs(block[index])))
                {
                    return true;
                }
            }
            values.Clear();
        }
        return false;
        */
    }

    // accept(P, c): return true if and only if candidate c is a solution of P
    public static bool Accept(Grid candidate)
    {
        // Check that all cells are set
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (candidate.GetVertex(x, y) == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }
    #endregion

    #region Extenders
    // first(P, c): generate the first extension of candidate c
    public static Grid? First(Grid data, Grid candidate)
    {
        // Make a shallow copy of the candidate
        Grid grid = candidate.ShallowCopy();

        // Update the copy
        for (int y = 0; y < Grid.HEIGHT; y++)
        {
            for (int x = 0; x < Grid.WIDTH; x++)
            {
                if (candidate.GetVertex(x, y) == 0) // Find the first cell with an unset value in the partial candidate
                {
                    // If the value is less than 9, increment it. Otherwise, continue to the next cell.
                    if (grid.GetVertex(x, y) < 9)
                    {
                        grid.SetVertex(x, y, candidate.GetVertex(x, y) + 1);
                        return grid;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        return null;
    }

    // next(P, s): generate the next extension of a candidate after the extension s.
    public static Grid? Next(Grid data, Grid candidate)
    {
        // Make a shallow copy of the candidate
        Grid grid = candidate.ShallowCopy();

        // Update the copy
        for (int y = 0; y < Grid.HEIGHT; y++)
        {
            for (int x = 0; x < Grid.WIDTH; x++)
            {
                if (data.GetVertex(x, y) == 0) // Find first cell with an unset value in the parent of the partial candidate
                {
                    // If the value is less than 9, increment it. Otherwise, continue to the next cell.
                    if (grid.GetVertex(x, y) < 9)
                    {
                        grid.SetVertex(x, y, candidate.GetVertex(x, y) + 1);
                        return grid;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        return null;
    }
    #endregion

}