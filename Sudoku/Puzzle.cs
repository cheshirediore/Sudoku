using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku;


// REFACTORING:
// replace the Cell[,] with a Grid object that will handle the translations
//      between the 2d array and 1d array. It will store only integer values.
// The Cell objects will be in a 1d array, where the index aligns with the
//      Grid object. This way, we can pass the Grid object down the recursion
//      stack for less overhead, and lookup cell data from the Cell[] when
//      it's needed. Given that it is not needed for the backtrack algorithm,
//      we can save memory usage there.



/// <summary>
/// Class <c>Puzzle</c> models the sudoku puzzle itself.
/// 
/// Blocks, Columns, and Rows are indexed 0-8. 
/// Blocks are indexed left to right, top to bottom. A block consists of a 3x3 group of cells.
/// Columns are indexed left to right.
/// Rows are indexed top to bottom.
/// </summary>
public class Puzzle
{

    private readonly Cell[,] _grid;

    public const int SIZE = 9;

    private HashSet<int> _clueIndices = new();

    // Although it's always a 9x9 square, using Width and Height can make it more readable in some places
    public int Width
    {
        get => SIZE;
    }
    public int Height
    {
        get => SIZE;
    }

    public Puzzle()
    {
        _grid = new Cell[Height, Width];
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                SetCell(x, y, new Cell(x, y));
            }
        }
    }

    #region Accessors
    public Cell GetCell(int x, int y)
    {
        return _grid[y, x];
    }

    public Cell GetCell(int index)
    {
        int[] coords = GetCellCoordinatesByIndex(index);
        return GetCell(coords[0], coords[1]);
    }

    // This is private because other object can update the cell's value, but shouldn't
    // be replacing the field value with a different instance of the reference type.
    private void SetCell(int x, int y, Cell newCell)
    {
        _grid[y, x] = newCell;
    }

    public int[] GetCellCoordinatesByIndex(int index)
    {
        int[] coordinates = [-1, -1];

        // x = (index % width)
        coordinates[0] = index % Width;
        // y = floor(index / width)
        coordinates[1] = (int)Math.Floor((double)(index / Width));

        return coordinates;
    }

    // Public accessors for the cell's value.
    public int GetValue(int x, int y)
    {
        return GetCell(x, y).Value;
    }

    public int GetValue(int index)
    {
        return GetCell(index).Value;
    }

    public bool SetValue(int x, int y, int newValue)
    {
        // Validate that the coordinates exist in the grid
        if (x < 0 || y < 0 || x > SIZE || y > SIZE)
        {
            return false;
        }
        // Validate that the value is within the allowed range
        if (Math.Abs(newValue) > 9)
        {
            return false;
        }

        GetCell(x, y).Value = newValue;

        return true;
    }

    public bool SetValue(int index, int newValue)
    {
        if (_clueIndices.Contains(index))
        {
            return false;
        }
        int[] coords = GetCellCoordinatesByIndex(index);
        return SetValue(coords[0], coords[1], newValue);
    }

    public bool IncrementValue(int index)
    {
        return SetValue(index, GetValue(index) + 1);
    }

    public bool ClearValue(int index)
    {
        return SetValue(index, 0);
    }

    public int GetPlayerValue(int x, int y)
    {
        return GetCell(x, y).PlayerValue;
    }

    public void SetPlayerValue(int x, int y, int newValue)
    {
        GetCell(x, y).PlayerValue = newValue;
    }

    public void SetPlayerValue(int index, int newValue)
    {
        int[] coords = GetCellCoordinatesByIndex(index);
        SetPlayerValue(coords[0], coords[1], newValue);
    }

    public void RegisterClue(int clueIndex)
    {
        _clueIndices.Add(clueIndex);
    }

    public bool IsClue(int index)
    {
        return _clueIndices.Contains(index);
    }

    /// <summary>
    /// <c>GetBlock</c> returns an array of <c>Cell</c> objects from a 3x3 block of the grid.
    /// Cells are ordered from the top left to the bottom right.
    /// </summary>
    public Cell[] GetBlock(int blockIndex)
    {
        // TODO: Handle out of bounds arguments
        Cell[] block = new Cell[9];

        /*
            A block is the intersection of (union of three columns) and (union of three rows)

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

        int[] columnIndices = new int[3];
        int[] rowIndices = new int[3];

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
        }

        // Iterate through the indices to get the cells in the block, as defined above
        int i = 0;
        for (int columnIndex = 0; columnIndex < columnIndices.Length; columnIndex++)
        {
            for (int rowIndex = 0; rowIndex < rowIndices.Length; rowIndex++)
            {
                block[i] = GetCell(columnIndices[columnIndex], rowIndices[rowIndex]);
                i++;
            }
        }
        return block;
    }

    public Cell[] GetRow(int rowIndex)
    {
        // TODO: Handle out of bounds arguments
        Cell[] row = new Cell[9];
        for (int x = 0; x < Width; x++)
        {
            row[x] = GetCell(x, rowIndex);
        }
        return row;
    }

    public Cell[] GetColumn(int columnIndex)
    {
        // TODO: Handle out of bounds arguments
        Cell[] column = new Cell[9];
        for (int y = 0; y < Height; y++)
        {
            column[y] = GetCell(columnIndex, y);
        }
        return column;
    }

    #endregion

    public void RevealCell(int x, int y)
    {
        SetPlayerValue(x, y, GetValue(x, y));
    }

    public void RevealCell(int index)
    {
        int[] coords = GetCellCoordinatesByIndex(index);
        RevealCell(coords[0], coords[1]);
    }

    public void RevealClues()
    {
        foreach (var clueIndex in _clueIndices)
        {
            RevealCell(clueIndex);
        }
    }

    public void PrintClueIndices()
    {
        Console.Write("[Puzzle] Clues: ");
        foreach (var clueIndex in _clueIndices)
        {
            Console.Write($"{clueIndex} ");
        }
        Console.WriteLine();
    }

    #region ValidationMethods
    // Check for conflicts only
    public bool IsConsistent()
    {
        for (int i = 0; i < SIZE; i++)
        {
            // Check columns
            int nonZeroValues = 0;
            var column = GetColumn(i);
            HashSet<int> distinctColumnValues = [];
            for (int index = 0; index < column.Length; index++)
            {
                if (column[index].Value != 0)
                {
                    nonZeroValues++;
                    distinctColumnValues.Add(column[index].Value);
                }
            }
            if (nonZeroValues != distinctColumnValues.Count)
            {
                PrintRegion(column);
                return false;
            }


            // Check rows
            nonZeroValues = 0;
            var row = GetRow(i);
            HashSet<int> distinctRowValues = [];
            for (int index = 0; index < row.Length; index++)
            {
                if (row[index].Value != 0)
                {
                    nonZeroValues++;
                    distinctRowValues.Add(row[index].Value);
                }
            }
            if (nonZeroValues != distinctRowValues.Count)
            {
                PrintRegion(row);
                return false;
            }


            // Check blocks
            nonZeroValues = 0;
            var block = GetBlock(i);
            HashSet<int> distinctBlockValues = [];
            for (int index = 0; index < block.Length; index++)
            {
                if (block[index].Value != 0)
                {
                    nonZeroValues++;
                    distinctBlockValues.Add(block[index].Value);
                }
            }
            if (nonZeroValues != distinctBlockValues.Count)
            {
                PrintRegion(block);
                return false;
            }
        }
        return true;
    }

    // Check that all cells are filled without conflicts
    public bool ValidateSolution()
    {
        // There are an equal number of columns, rows, and blocks, so we can
        // use the same incrementing value to check all three each iteration.
        for (int i = 0; i < 9; i++)
        {
            // Check the column, row, and block. If any are invalid, update the attribute to false and return. 
            // Otherwise, keep iterating.
            if (!ValidateRegionGroup(i))
            {
                return false;
            }
        }
        // If it makes it to the end without finding an invalid region, then it's a valid solution.
        return true;
    }

    // Here, a "Region Group" means regions sharing the same index
    // Returns true if and only if all regions sharing the index are valid
    private bool ValidateRegionGroup(int regionIndex)
    {
        return ValidateRegion(GetColumn(regionIndex))
                && ValidateRegion(GetRow(regionIndex))
                && ValidateRegion(GetBlock(regionIndex));
    }

    public static bool ValidateRegion(Cell[] region)
    {
        // Use a set to determine if there are duplicate values 
        HashSet<int> validationSet = [];

        for (int i = 0; i < region.Length; i++)
        {

            validationSet.Add(region[i].PlayerValue);
        }

        // If the number of elements in the set are different
        // than the number of cells in the region, then there
        // must be duplicates (or skipped invalid values).
        return region.Length == validationSet.Count;
    }


    public void IncrementCellValue(int index)
    {
        int cellValue = GetValue(index);
        cellValue = (cellValue + 1) % 9;
        SetValue(index, cellValue);
    }
    #endregion


    public int[] GetEmptyCellIndices()
    {
        int numberOfEmptyCells = 0;
        for (int i = 0; i < 81; i++)
        {
            if (GetValue(i) == 0)
            {
                numberOfEmptyCells++;
            }
        }
        int[] emptyCells = new int[numberOfEmptyCells];

        int insertionIndex = 0;
        for (int i = 0; i < 81; i++)
        {
            if (GetValue(i) == 0)
            {
                emptyCells[insertionIndex] = i;

                insertionIndex++;
            }
        }
        return emptyCells;
    }


    #region DebugMethods
    public void PrintRegion(Cell[] region)
    {
        StringBuilder builder = new();
        builder.Append("[");
        for (int i = 0; i < region.Length; i++)
        {
            builder.Append($"{region[i].Value}");
            if (i < region.Length - 1) builder.Append(", ");
        }
        builder.Append("]");

    }

    public string PrintPuzzle()
    {
        AsciiGrid asciiGrid = new(Width, Height);
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                string cellValue = $"{GetValue(x, y)}";
                asciiGrid.SetGridCell(x, y, cellValue);
            }
        }
        return asciiGrid.ToString();
    }

    public override string ToString()
    {
        return PrintPuzzle();
    }
    #endregion
}