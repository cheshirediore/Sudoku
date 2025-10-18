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

    private int[] _dimensions = new int[2];
    private Cell[,] _grid;

    public int Width
    {
        get => _dimensions[0];
        set => _dimensions[0] = value;
    }
    public int Height
    {
        get => _dimensions[1];
        set => _dimensions[1] = value;
    }


    public Puzzle(int width, int height)
    {
        Width = width;
        Height = height;
        _grid = new Cell[Width, Height];
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _grid[x, y] = new Cell();
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        return _grid[x, y];
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
}