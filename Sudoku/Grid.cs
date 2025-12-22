using System.Text;

namespace Sudoku;

public class Grid
{
    const int WIDTH = 9;
    const int HEIGHT = 9;

    public readonly int[][] vertices;

    public Grid()
    {
        // Initialize (the list of rows part of) the jagged array 
        vertices = new int[HEIGHT][];

        // Initialize rows, and set all values to 0 (the default integer value, conveniently)
        for (int y = 0; y < HEIGHT; y++)
        {
            // Initialize an individual row of WIDTH length in the jagged array
            vertices[y] = new int[WIDTH];
        }
    }

    public Grid(int[][] gridVertices)
    {
        vertices = gridVertices;
    }

    public static int[] IndexToCoordinates(int index, int width)
    {
        int[] coordinates = [-1, -1];

        // x = (index % width)
        coordinates[0] = index % width;
        // y = index / width (integer division)
        coordinates[1] = index / width;

        return coordinates;
    }
    
    /// <summary>
    /// Method used to translate familiar (x, y) cartesian coordinate notation to [row, column] 2D array indices.
    /// </summary>
    public int GetVertex(int x, int y)
    {
        return vertices[y][x];
    }

    /// <summary>
    /// Method used to translate a single integer index value to a pair of indices for a 2D array.
    /// </summary>
    public int GetVertex(int index)
    {
        int[] coords = IndexToCoordinates(index, WIDTH);
        return GetVertex(coords[0], coords[1]);
    }

    public void SetVertex(int index, int value)
    {
        int[] coordinates = IndexToCoordinates(index, WIDTH);
        int x = coordinates[0];
        int y = coordinates[1];

        vertices[y][x] = value;
    }

    /// <summary>
    /// Method to get a horizontal slice of a 2D array
    /// </summary>
    public int[] GetRow(int rowIndex)
    {
        int[] row = new int[9];
        for (int x = 0; x < 9; x++)
        {
            row[x] = GetVertex(x, rowIndex);
        }
        return row;
    }

    /// <summary>
    /// Method to get a vertical slice of a 2D array
    /// </summary>
    public int[] GetColumn(int columnIndex)
    {
        // TODO: Handle out of bounds arguments
        int[] column = new int[9];
        for (int y = 0; y < 9; y++)
        {
            column[y] = GetVertex(columnIndex, y);
        }
        return column;
    }

    /// <summary>
    /// Method to get 3x3 vector from a 2D array, transformed to a 1x9 vector.
    /// </summary>
    public int[] GetBlock(int blockIndex)
    {
        // TODO: Handle out of bounds arguments
        int[] block = new int[9];

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
                block[i] = GetVertex(columnIndices[columnIndex], rowIndices[rowIndex]);
                i++;
            }
        }
        return block;
    }

    public override string ToString()
    {
        StringBuilder builder = new();

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                builder.Append($"{vertices[y][x]} ");
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}