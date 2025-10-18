using System.Dynamic;
using System.Reflection.Metadata;
using System.Threading.Channels;

namespace Sudoku;

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
    private bool _isSolved = false;

    public const int SIZE = 9;
    
    public bool IsSolved
    {
        get => _isSolved;
    }

    // Although it's always a 9x9 square, using Width and Height can make it more readable in some places
    public static int Width
    {
        get => SIZE;
    }
    public static int Height
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
                SetCell(x, y, new Cell());
            }
        }
    }

    // This is private because other object can update the cell's value, but shouldn't
    // be replacing the field value with a different instance of the reference type.
    private void SetCell(int x, int y, Cell newCell)
    {
        _grid[y, x] = newCell;
    }

    private Cell GetCell(int x, int y)
    {
        return _grid[y, x];
    }

    // Public accessors for the cell's value.
    public void SetValue(int x, int y, int newValue)
    {
        GetCell(x, y).Value = newValue;
    }
    public int GetValue(int x, int y)
    {
        return GetCell(x, y).Value;
    }
    public void SetPlayerValue(int x, int y, int newValue)
    {
        GetCell(x, y).PlayerValue = newValue;
    }
    public int GetPlayerValue(int x, int y)
    {
        return GetCell(x, y).PlayerValue;
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

    public void ValidateSolution()
    {
        // There are an equal number of columns, rows, and blocks, so we can
        // use the same incrementing value to check all three each iteration.
        for (int i = 0; i < 9; i++)
        {
            // Check the column, row, and block. If any are invalid, update the attribute to false and return. 
            // Otherwise, keep iterating.
            if (!ValidateRegionGroup(i))
            {
                _isSolved = false;
                return;
            }
        }
        // If it makes it to the end without finding an invalid region, then it's a valid solution.
        _isSolved = true;
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
}