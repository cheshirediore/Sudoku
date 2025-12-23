using System;
using System.Collections.Generic;

namespace Sudoku;

public class Backtracker
{
    #region Algorithm
    // From wikipedia:
    // P is the data
    // c is a partial candidate

    // backtrack(P, c)
    // procedure backtrack(P, c) is
    // if reject(P, c) then return
    // if accept(P, c) then output(P, c)
    // s ← first(P, c)
    // while s ≠ NULL do
    //     backtrack(P, s)
    //     s ← next(P, s)

    public static List<int[][]> Backtrack(int[][] data, int[][] candidate, List<int[][]> solutions)
    {
        return Backtrack(data, candidate, solutions, 0);
    }

    public static List<int[][]> Backtrack(int[][] data, int[][] candidate, List<int[][]> solutions, int depth)
    {
        // Console.WriteLine($"Backtrack {depth}");
        // Console.WriteLine($"Data:");
        // Console.WriteLine(new AsciiGrid(data, depth));
        // Console.WriteLine($"Candidate:");
        // Console.WriteLine(new AsciiGrid(candidate, depth));
        if (Reject(data, candidate))
        {
            // Console.WriteLine($"Rejected (Depth {depth})");
            // Console.WriteLine(new AsciiGrid(candidate, depth));
            return solutions;
        }
        if (Accept(data, candidate))
        {
            // Console.WriteLine($"Accepted (Depth {depth})");
            // Console.WriteLine(new AsciiGrid(candidate, depth));
            return Output(candidate, solutions);
        }

        int[][]? nextCandidate = First(data, candidate);
        while (nextCandidate != null)
        {
            solutions = Backtrack(candidate, nextCandidate, solutions, depth+1);
            nextCandidate = Next(candidate, nextCandidate);
            // if (nextCandidate != null)
            // {
            //     Console.WriteLine($"Next Candidate (Depth {depth})");
            //     Console.WriteLine(new AsciiGrid(nextCandidate, depth));
            // }
        }
        return solutions;
    }
    #endregion

    // root(P): return the partial candidate at the root of the search tree
    public static int[][] Root(int[][] data)
    {
        return data;
    }

    #region Validation
    // reject(P, c): return true only if the partial candidate c is not worth completing
    public static bool Reject(int[][] data, int[][] candidate)
    {
        for (int i = 0; i < 9; i++)
        {
            // Check columns
            var column = GetColumnValues(candidate, i);
            HashSet<int> values = new();
            
            for (int index = 0; index < column.Length; index++)
            {
                if (column[index] != 0 && !values.Add(Math.Abs(column[index])))
                {
                    return true;
                }
            }
            values.Clear();

            // Check rows
            var row = GetRowValues(candidate, i);
            for (int index = 0; index < row.Length; index++)
            {
                if (row[index] != 0 && !values.Add(Math.Abs(row[index])))
                {
                    return true;
                }
            }
            values.Clear();


            // Check blocks
            var block = GetBlockValues(candidate, i);
            for (int index = 0; index < block.Length; index++)
            {
                if (block[index] != 0 && !values.Add(Math.Abs(block[index])))
                {
                    return true;
                }
            }
            values.Clear();
        }
        return false;
    }

    // accept(P, c): return true if and only if candidate c is a solution of P
    public static bool Accept(int[][] data, int[][] candidate)
    {
        // Check that all cells are set
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (candidate[y][x] == 0)
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
    public static int[][]? First(int[][] data, int[][] candidate)
    {
        if (candidate.Length < 1)
        {
            return null;
        }
        // Make a shallow copy of the candidate
        int[][] grid = MakeShallowCopy(candidate);

        // Update the copy
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[0].Length; x++)
            {
                if (candidate[y][x] == 0) // Find the first cell with an unset value in the partial candidate
                {
                    // If the value is less than 9, increment it. Otherwise, continue to the next cell.
                    if (grid[y][x] < 9)
                    {
                        grid[y][x] = candidate[y][x] + 1;
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
    public static int[][]? Next(int[][] data, int[][] candidate)
    {
        if (candidate.Length < 1)
        {
            return null;
        }
        // Make a shallow copy of the candidate
        int[][] grid = MakeShallowCopy(candidate);

        // Update the copy
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[0].Length; x++)
            {
                if (data[y][x] == 0) // Find first cell with an unset value in the parent of the partial candidate
                {
                    // If the value is less than 9, increment it. Otherwise, continue to the next cell.
                    if (grid[y][x] < 9)
                    {
                        grid[y][x] = candidate[y][x] + 1;
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

    // output(P, c): use the solution c of P, as appropriate to the application
    public static List<int[][]> Output(int[][] candidate, List<int[][]> solutions)
    {
        solutions.Add(candidate);
        return solutions;
    }

    #region HelperFunctions
    public static int[] GetColumnValues(int[][] data, int columnIndex)
    {
        int[] column = new int[9];
        for (int i = 0; i < 9; i++)
        {
            column[i] = data[i][columnIndex];
        }
        return column;
    }

    public static int[] GetRowValues(int[][] data, int rowIndex)
    {
        int[] row = new int[9];
        for (int i = 0; i < 9; i++)
        {
            row[i] = data[rowIndex][i];
        }
        return row;
    }

    public static int[] GetBlockValues(int[][] data, int blockIndex)
    {
        int[] block = new int[9];

        /*
            A block is the intersection of the (union of three columns) and the (union of three rows).
            We store the values as an array, despite viewing it as a set operation, because we need to keep
            duplicate values.

            Block 0
            (columns 0, 1, 2) intersect (rows 0, 1, 2)
            Block 1
            (columns 3, 4, 5) intersect (rows 0, 1, 2)
            Block 2
            (columns 6, 7, 8) intersect (rows 0, 1, 2)

            Block 3
            (columns 0, 1, 2) intersect (rows 3, 4, 5)
            Block 4
            (columns 3, 4, 5) intersect (rows 3, 4, 5)
            Block 5
            (columns 6, 7, 8) intersect (rows 3, 4, 5)

            Block 6
            (columns 0, 1, 2) intersect (rows 6, 7, 8)
            Block 7
            (columns 3, 4, 5) intersect (rows 6, 7, 8)
            Block 8
            (columns 6, 7, 8) intersect (rows 6, 7, 8)
        */

        int[] columnIndices;
        int[] rowIndices;

        // Give useful names to the index groups
        int[] FIRST = [0, 1, 2];
        int[] SECOND = [3, 4, 5];
        int[] THIRD = [6, 7, 8];

        // Only 9x9 grids are supported, so we can hard code these cases instead of calculating them at runtime
        switch (blockIndex)
        {
            // Top row of blocks
            case 0:
                columnIndices = FIRST;
                rowIndices = FIRST;
                break;
            case 1:
                columnIndices = SECOND;
                rowIndices = FIRST;
                break;
            case 2:
                columnIndices = THIRD;
                rowIndices = FIRST;
                break;
            // Middle row of blocks
            case 3:
                columnIndices = FIRST;
                rowIndices = SECOND;
                break;
            case 4:
                columnIndices = SECOND;
                rowIndices = SECOND;
                break;
            case 5:
                columnIndices = THIRD;
                rowIndices = SECOND;
                break;
            // Bottom row of blocks
            case 6:
                columnIndices = FIRST;
                rowIndices = THIRD;
                break;
            case 7:
                columnIndices = SECOND;
                rowIndices = THIRD;
                break;
            case 8:
                columnIndices = THIRD;
                rowIndices = THIRD;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(blockIndex), $"blockIndex must be an integer between 0 and 8 (inclusive). Received '{blockIndex}'.");
        }

        // Iterate through the indices to get the cells in the block, as defined above
        int i = 0;
        for (int columnIndex = 0; columnIndex < columnIndices.Length; columnIndex++)
        {
            int x = columnIndices[columnIndex];
            for (int rowIndex = 0; rowIndex < rowIndices.Length; rowIndex++)
            {
                int y = rowIndices[rowIndex];
                block[i] = data[y][x];
                i++;
            }
        }
        return block;
    }

    public static int[][] MakeShallowCopy(int[][] candidate)
    {
        // Make a shallow copy of the candidate
        int[][] grid = new int[candidate.Length][];
        for (int y = 0; y < candidate[0].Length; y++)
        {
            grid[y] = new int[candidate[y].Length];
            for (int x = 0; x < candidate[y].Length; x++)
            {
                grid[y][x] = candidate[y][x];
            }
        }
        return grid;
    }
    #endregion
}