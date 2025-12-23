namespace Sudoku;

public class Grid
{
    const int WIDTH = 9;
    const int HEIGHT = 9;

    // Give useful names to the index groups used for blocks
    private readonly int[] FIRST = [0, 1, 2];
    private readonly int[] SECOND = [3, 4, 5];
    private readonly int[] THIRD = [6, 7, 8];

    public readonly int[][] Vertices;

    public Grid()
    {
        // Initialize (the list of rows part of) the jagged array 
        Vertices = new int[HEIGHT][];

        // Initialize rows, and set all values to 0 (the default integer value, conveniently)
        for (int y = 0; y < HEIGHT; y++)
        {
            // Initialize an individual row of WIDTH length in the jagged array
            Vertices[y] = new int[WIDTH];
        }
    }

    public Grid(int[][] gridVertices)
    {
        Vertices = gridVertices;
    }

    public Grid(string seedFilePath)
    {
        // Open the file, read the content, and close it
        string fileContent = System.IO.File.ReadAllText(seedFilePath);

        // Split the content by lines
        string[] lines = fileContent.Split("\n");
        if (lines.Length != HEIGHT)
        {
            throw new System.ArgumentOutOfRangeException(seedFilePath, $"Input puzzle seed must have {HEIGHT} lines. Provided seed has '{lines.Length}'.");
        }
        // Initialize vertex grid
        Vertices = new int[HEIGHT][];

        // Iterate over the lines and add the values to the vertex grid
        for (int y = 0; y < HEIGHT; y++)
        {
            // Create the row in the vertex grid
            Vertices[y] = new int[WIDTH];

            // Split the line by commas, and trim off the whitespace
            string[] rowValues = lines[y].Split(",");
            // Verify that the width is correct before adding it to the vertices
            if (rowValues.Length != WIDTH)
            {
                throw new System.ArgumentOutOfRangeException(seedFilePath, $"'{rowValues.Length}' is an invalid width. All rows in the input puzzle seed must have a width of {WIDTH}.");
            }
            // Verify that each string is numeric, and add it to the vertices iff it is. Otherwise, throw an exception. TODO: change line 104 into a verbose IF/ELSE block with tha exception
            for (int x = 0; x < rowValues.Length; x++)
            {
                if (int.TryParse(rowValues[x].Trim(), out int parsedValue))
                {
                    // Using negative numbers to flag the clue values using a single int
                    Vertices[y][x] = parsedValue * -1;
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException(seedFilePath, $"Invalid value passed in puzzle seed. Check file for non-numeric characters.");
                }
            }
        }
    }

    public static int[] IndexToCoordinates(int index)
    {
        int[] coordinates = [-1, -1];

        // x = (index % width)
        coordinates[0] = index % WIDTH;
        // y = index / width (integer division)
        coordinates[1] = index / WIDTH;

        return coordinates;
    }
    
    /// <summary>
    /// Method used to translate familiar (x, y) cartesian coordinate notation to [row, column] 2D array indices.
    /// </summary>
    public int GetVertex(int x, int y)
    {
        return Vertices[y][x];
    }

    /// <summary>
    /// Method used to translate a single integer index value to a pair of indices for a 2D array.
    /// </summary>
    public int GetVertex(int index)
    {
        int[] coords = IndexToCoordinates(index);
        return GetVertex(coords[0], coords[1]);
    }

    public void SetVertex(int x, int y, int value)
    {
        Vertices[y][x] = value;
    }

    public void SetVertex(int index, int value)
    {
        int[] coordinates = IndexToCoordinates(index);

        SetVertex(coordinates[0], coordinates[1], value);
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
        System.Text.StringBuilder builder = new();

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                builder.Append($"{Vertices[y][x]} ");
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}