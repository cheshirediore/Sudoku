using System;
using System.Collections.Immutable;
using System.Collections.Generic;

namespace Sudoku;

public class Grid: IEquatable<Grid>
{
    // Static Constants //
    // Only a 9x9 grid is supported by this implementation. WIDTH, HEIGHT, and SIZE are provided for clean reference by other objects.
    public const int WIDTH = 9;
    public const int HEIGHT = 9;
    public static int SIZE { get => WIDTH * HEIGHT; }

    // Give useful names to the index groups used for blocks. FIRST, SECOND, and THIRD can refer to either a group of columns or rows.
    // These are used exclusively by the GetBlock method.
    private readonly ImmutableArray<int> FIRST = [0, 1, 2];
    private readonly ImmutableArray<int> SECOND = [3, 4, 5];
    private readonly ImmutableArray<int> THIRD = [6, 7, 8];

    // Instance Attributes //
    public readonly int[][] Vertices;
    private int _lastUpdatedCellIndex = 0;

    #region Constructors
    /// <summary>
    /// Default constructor. Creates a Grid object where all vertices are empty.
    /// </summary>
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

    /// <summary>
    /// Creates a Grid object from an existing array of integer array.
    /// </summary>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when <paramref name="gridVertices"/> is not of the expected dimensions.
    /// </exception>
    public Grid(int[][] gridVertices)
    {
        if (gridVertices.Length != HEIGHT)
        {
            throw new System.ArgumentOutOfRangeException(nameof(gridVertices), $"int[{gridVertices.Length}][] is an invalid length. Array must be of length {HEIGHT}.");
        }
        for (int y = 0; y < HEIGHT; y++)
        {
            if (gridVertices[y].Length != WIDTH)
            {
                throw new System.ArgumentOutOfRangeException(nameof(gridVertices), $"int[{y}][{gridVertices[y].Length}] is an invalid length. Arrays must be of length {WIDTH}.");
            }
        }
        Vertices = gridVertices;
    }

    /// <summary>
    /// Creates a Grid object using a provided file.
    /// </summary>
    /// <param name="seedFilePath">
    /// File path should indicate a csv file with 9 lines, and 9 columns. Each column must contain an integer
    /// value between 0 and 8 (inclusive), where 0 indicates an unset value.
    /// </param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when the input file is not the appropriate width, height, or when it contains non-numeric values.
    /// </exception>
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
    #endregion

    #region Static Methods

    /// <summary>
    /// Converts a pair of coordinates to a cell index.
    /// </summary>
    /// <param name="coordinates">
    /// A cartesian coordinate pair indicating the location of the cell on a cartesian plan.
    /// </param>
    /// <returns>
    /// An integer value indicating the number of the cell as counted from top left to bottom right of the grid.
    /// </returns>
    /// <remarks>
    /// Does not check if the given corrodinates (and corresponding index) are valid for a Grid.
    /// </remarks>
    public static int CoordinatesToIndex(int[] coordinates)
    {
        return coordinates[1] * WIDTH + coordinates[0];
    }

    /// <summary>
    /// Converts a cell index to a pair of coordinates.
    /// </summary>
    /// <param name="index">
    /// An integer value indicating the number of the cell as counted from top left to bottom right of the grid.
    /// </param>
    /// <returns>
    /// The cartesian coordinate pair of the cell indicated by the <paramref name="index"/>.
    /// </returns>
    /// <remarks>
    /// Does not check if the given index (and corresponding pair of coordinates) is valid for a Grid.
    /// </remarks>
    public static int[] IndexToCoordinates(int index)
    {
        int[] coordinates = [-1, -1];

        // x = (index % width)
        coordinates[0] = index % WIDTH;
        // y = index / width (integer division)
        coordinates[1] = index / WIDTH;
        // // y = floor(index / width)
        // coordinates[1] = (int)Math.Floor((double)(index / WIDTH));

        return coordinates;
    }

    /// <summary>
    /// Method to translate a given pair of coordinates to block index.
    /// </summary>
    /// <param name="x">
    /// The x coordinate of the vertex.
    /// </param>
    /// <param name="y">
    /// The y coordinate of the vertex. 
    /// </param>
    /// <returns>
    /// An index indicating which block contains the given pair of coordinates.
    /// </returns>
    public static int GetBlockIndex(int x, int y)
    {
        /*
              0  1  2    3  4  5    6  7  8

        0     0  1  2 |  3  4  5 |  6  7  8
        1     9 10 11 | 12 13 14 | 15 16 17
        2    18 19 20 | 21 22 23 | 24 25 26
             ------------------------------
        3    27 28 29 | 30 31 32 | 33 34 35
        4    36 37 38 | 39 40 41 | 42 43 44
        5    45 46 47 | 48 49 50 | 51 52 53
             ------------------------------
        6    54 55 56 | 57 58 59 | 60 61 62
        7    63 64 65 | 66 67 68 | 69 70 71
        8    72 73 74 | 75 76 77 | 78 79 80
        */

        // Not a very elegant solution, but it works.

        // Top 3 Rows
        if (y < 3)
        {
            // Left 3 columns
            if (x < 3)
            {
                return 0;
            }
        
            // Middle 3 columns
            if (x > 2 && x < 6)
            {
                return 1;
            }

            // Right 3 columns
            if (x > 5)
            {
                return 2;
            }
        }

        // Middle 3 Rows
        if (y > 2 && y < 6 )
        {
            // Left 3 columns
            if (x < 3)
            {
                return 3;
            }
        
            // Middle 3 columns
            if (x > 2 && x < 6)
            {
                return 4;
            }

            // Right 3 columns
            if (x > 5)
            {
                return 5;
            }
        }

        // Bottom 3 Rows
        if (y > 5)
        {
            // Left 3 columns
            if (x < 3)
            {
                return 6;
            }
        
            // Middle 3 columns
            if (x > 2 && x < 6)
            {
                return 7;
            }

            // Right 3 columns
            if (x > 5)
            {
                return 8;
            }
        }
        return -1;
    }

    /// <summary>
    /// Translates a given cell index to a block index.
    /// </summary>
    /// <param name="cellIndex"></param>
    /// <returns>
    /// Returns the index of the block containing the given cell index.
    /// </returns>
    public static int GetBlockIndex(int cellIndex)
    {
        int[] coordinates = IndexToCoordinates(cellIndex);  
        return GetBlockIndex(coordinates[0], coordinates[1]);   
    }
    #endregion Static Methods
    
    #region GetVertex Overloads
    /// <summary>
    /// Primary accessor method for getting the value of a given vertex.
    /// Method used to translate familiar (x, y) cartesian coordinate notation to [row, column] 2D array indices.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when either <paramref name="x"/> or <paramref name="y"/> are less than 0 or greater than 8.
    /// </exception>
    public int GetVertex(int x, int y)
    {
        if (x < 0 || x > 8)
        {
            throw new System.ArgumentOutOfRangeException(nameof(x), $"({x}, {y}) is outside of the grid bounds.");
        }
        if (y < 0 || y > 8)
        {
            throw new System.ArgumentOutOfRangeException(nameof(y), $"({x}, {y}) is outside of the grid bounds.");
        }
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
    #endregion GetVertex Overloads

    #region SetVertex Overloads
    /// <summary>
    /// Primary accessor method for setting the value of a given vertex.
    /// Method used to translate familiar (x, y) cartesian coordinate notation to [row, column] 2D array indices. 
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when either <paramref name="x"/> or <paramref name="y"/> are less than 0 or greater than 8.
    /// </exception>
    /// <param name="x">
    /// The x coordinate of the vertex.
    /// </param>
    /// <param name="y">
    /// The y coordinate of the vertex. 
    /// </param>
    /// <param name="value">
    /// The new value for the vertex.
    public void SetVertex(int x, int y, int value)
    {
        if (x < 0 || x > 8)
        {
            throw new System.ArgumentOutOfRangeException(nameof(x), $"({x}, {y}) is outside of the grid bounds.");
        }
        if (y < 0 || y > 8)
        {
            throw new System.ArgumentOutOfRangeException(nameof(y), $"({x}, {y}) is outside of the grid bounds.");
        }
        _lastUpdatedCellIndex = CoordinatesToIndex([x, y]);
        Vertices[y][x] = value;
    }

    /// <summary>
    /// Overload of SetVertex(int x, int y, int value) to support the <paramref name="isSeedValue"/> parameter.
    /// </summary>
    /// <param name="x">
    /// The x coordinate of the vertex.
    /// </param>
    /// <param name="y">
    /// The y coordinate of the vertex. 
    /// </param>
    /// <param name="value">
    /// The new value for the vertex.
    /// </param>
    /// <param name="isSeedValue">
    /// Flag indicating whether the vertex should be considered a seed value.
    /// </param>
    public void SetVertex(int x, int y, int value, bool isSeedValue)
    {
        if (isSeedValue)
        {
            SetVertex(x, y, Math.Abs(value) * -1);
        }
        else
        {
            SetVertex(x, y, value);
        }
    }

    public void SetVertex(int index, int value)
    {
        int[] coordinates = IndexToCoordinates(index);

        SetVertex(coordinates[0], coordinates[1], value);
    }

    public void SetVertex(int index, int value, bool isSeedValue)
    {
        if (isSeedValue)
        {
            SetVertex(index, Math.Abs(value) * -1);
        }
        else
        {
            SetVertex(index, value);
        }
    }
    #endregion SetVertex Overloads

    #region Validity Checking Methods
    /// <summary>
    /// Method to check the consistency of the row, column, and block that contain the last updated vertex.
    /// </summary>
    /// <returns>
    /// Returns true if and only if the row, column, and block that contain the last updated vertex are consistent.
    /// </returns>
    public bool IsLastUpdateValid()
    {
        return IsLastUpdatedRowValid() && IsLastUpdatedColumnValid() && IsLastUpdatedBlockValid();
    }

    /// <summary>
    /// Method to check the consistency of the row that contains the last updated vertex.
    /// </summary>
    /// <returns>
    /// Returns true if and only if the row that contains the last updated vertex is consistent.
    /// </returns>
    public bool IsLastUpdatedRowValid()
    {
        int[] coordinates = IndexToCoordinates(_lastUpdatedCellIndex);
        return IsRowConsistent(coordinates[1]);
    }

    /// <summary>
    /// Method to check the consistency of the column that contains the last updated vertex.
    /// </summary>
    /// <returns>
    /// Returns true if and only if the column that contains the last updated vertex is consistent.
    /// </returns>
    public bool IsLastUpdatedColumnValid()
    {
        int[] coordinates = IndexToCoordinates(_lastUpdatedCellIndex);
        return IsColumnConsistent(coordinates[0]);
    }

    /// <summary>
    /// Method to check the consistency of the block that contains the last updated vertex.
    /// </summary>
    /// <returns>
    /// Returns true if and only if the block that contains the last updated vertex is consistent.
    /// </returns>
    public bool IsLastUpdatedBlockValid()
    {
        int[] coordinates = IndexToCoordinates(_lastUpdatedCellIndex);
        return IsBlockConsistent(coordinates[0], coordinates[1]);
    }

    /// <summary>
    /// Method to check the consistency of the indicated row.
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <returns>
    /// Returns true if and only if the given row does not contain any non-zero duplicate values.
    /// </returns>
    public bool IsRowConsistent(int rowIndex)
    {
        HashSet<int> values = new();

        var row = GetRow(rowIndex);
        for (int index = 0; index < row.Length; index++)
        {
            if (row[index] != 0 && !values.Add(System.Math.Abs(row[index])))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Method to check the consistency of the indicated column.
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <returns>
    /// Returns true if and only if the given column does not contain any non-zero duplicate values.
    /// </returns>
    public bool IsColumnConsistent(int columnIndex)
    {
        HashSet<int> values = new();

        var column = GetColumn(columnIndex);
        for (int index = 0; index < column.Length; index++)
        {
            if (column[index] != 0 && !values.Add(System.Math.Abs(column[index])))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Method to check the consistency of the block containing the indicated vertex.
    /// </summary>
    /// <param name="x">
    /// The x coordinate of the given vertex.
    /// </param>
    /// <param name="y">
    /// The y coordinate of the given vertex.
    /// </param>
    /// <returns>
    /// Returns true if and only if the block that contains the given vertex is consistent.
    /// </returns>
    public bool IsBlockConsistent(int x, int y)
    {
        return IsBlockConsistent(GetBlockIndex(x, y));
    }

    /// <summary>
    /// Method to check the consistency of the indicated block.
    /// </summary>
    /// <param name="blockIndex"></param>
    /// <returns>
    /// Returns true if and only if the given block does not contain any non-zero duplicate values.
    /// </returns>
    public bool IsBlockConsistent(int blockIndex)
    {
        HashSet<int> values = new();

        var block = GetBlock(blockIndex);
        for (int index = 0; index < block.Length; index++)
        {
            if (block[index] != 0 && !values.Add(System.Math.Abs(block[index])))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Method to check the consistency of the entire grid.
    /// </summary>
    /// <returns>
    /// Returns true if and only if all rows, columns, and blocks are consistent.
    /// </returns>
    public bool IsGridConsistent()
    {
        for (int i = 0; i < 9; i++)
        {
            if (!IsRowConsistent(i) || !IsColumnConsistent(i) || !IsBlockConsistent(i))
            {
                return false;
            }
        }
        return true;
    }
    #endregion Validity Checking Methods

    #region Grid Region Accessor Methods
    /// <summary>
    /// Method to get the horizontal slice of a 2D array representing a grid row.
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
    /// Method to get the vertical slice of a 2D array representing a grid column.
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
    /// <param name="blockIndex">
    /// The index of a block, where each block is a 3x3 group of vertices numbered left to right, top to bottom.
    /// </param>
    /// <returns>
    /// The collection of cell indices that are within the block at the provided <paramref name="blockIndex"/>
    /// </returns>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    /// <remarks>
    /// A block is the intersection of (union of three columns) and (union of three rows)
    ///     Block 0
    ///     (columns 0, 1, 2) intersect (rows 0, 1, 2)
    ///     Block 1
    ///     (columns 3, 4, 5) intersect (rows 0, 1, 2)
    ///     Block 2
    ///     (columns 6, 7, 8) intersect (rows 0, 1, 2)
    /// 
    ///     Block 3
    ///     (columns 0, 1, 2) intersect (rows 3, 4, 5)
    ///     Block 4
    ///     (columns 3, 4, 5) intersect (rows 3, 4, 5)
    ///     Block 5
    ///     (columns 6, 7, 8) intersect (rows 3, 4, 5)
    /// 
    ///     Block 6
    ///     (columns 0, 1, 2) intersect (rows 6, 7, 8)
    ///     Block 7
    ///     (columns 3, 4, 5) intersect (rows 6, 7, 8)
    ///     Block 8
    ///     (columns 6, 7, 8) intersect (rows 6, 7, 8)
    /// <remarks/>
    public int[] GetBlock(int blockIndex)
    {
        int[] block = new int[9];

        ImmutableArray<int> columnIndices;
        ImmutableArray<int> rowIndices;
        
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
                throw new System.ArgumentOutOfRangeException(nameof(blockIndex), $"blockIndex must be an integer between 0 and 8 (inclusive). Received '{blockIndex}'.");
        }

        // Iterate through the indices to get the cells in the block, as defined above.
        int i = 0;
        for (int columnIndex = 0; columnIndex < columnIndices.Length; columnIndex++)
        {
            int x = columnIndices[columnIndex];
            for (int rowIndex = 0; rowIndex < rowIndices.Length; rowIndex++)
            {
                int y = rowIndices[rowIndex];
                block[i] = GetVertex(x, y);
                i++;
            }
        }
        return block;
    }
    #endregion Grid Region Accessor Methods

    
    #region Utility
    /// <summary>
    /// Memberwise copy method.
    /// </summary>
    /// <returns>
    /// Returns a new Grid object with the same values for each vertices as the original Grid.
    /// </returns>
    public Grid ShallowCopy()
    {
        // Make a shallow copy of the candidate
        Grid newGrid = new();
        for (int y = 0; y < HEIGHT; y++)
        {
            Array.Copy(Vertices[y], newGrid.Vertices[y], Grid.WIDTH);
        }
        return newGrid;
    }
    #endregion Utility

    #region Overrides and Interface
    /// <summary>
    /// Formats an ascii grid of the numeric values in the grid. If the number is positive, it adds a 
    /// space prefix to the cell to maintain a fixed width for integers between -9 and 9 (inclusive).
    /// </summary>
    /// <returns>A string representation of a HEIGHT x WIDTH matrix of the values in the grid, cleanly formatted for single digit signed integers.</returns>
    public override string ToString()
    {
        System.Text.StringBuilder builder = new();

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                string prefix = Vertices[y][x] < 0 ? "": " ";
                builder.Append($"{prefix}{Vertices[y][x]} ");
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }

    /// <summary>
    /// A Grid is considered equal to another Grid if it is not null and has the same hashcode.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>
    /// Returns true if and only if both Grid objects are not null and have the same hashcode.
    /// </returns>
    public bool Equals(Grid? other)
    {
        return other != null && GetHashCode() == other.GetHashCode();
    }

    /// <summary>
    /// Override method for IEquatable interface.
    /// </summary>
    /// <param name="obj">
    /// The object to be compared with <c>this<c/>.
    /// </param>
    /// <returns>
    /// Returns the result of Equals(Grid? other)
    /// </returns>
    /// <see cref="Equals"/>
    public override bool Equals(object? obj)
    {
        return obj != null && Equals(obj as Grid);
    }

    /// <summary>
    /// Combines all values of vertices into a hashcode.
    /// </summary>
    /// <returns>
    /// The hash representing this object.
    /// </returns>
    public override int GetHashCode()
    {
        // Use the dimensions for the initial hash seed. If the size becomes variable, it will matter. If it doesn't,
        // then we'll still have a more interesting seed than 0.
        // Values are multiplied by 100 to improve diffusion of the resulting hash.
        int hash = HashCode.Combine(WIDTH * 100, HEIGHT * 100);
        for (int y = 0; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                hash = HashCode.Combine(hash, GetVertex(x * 100, y * 100));
            }
        }
        return hash;
    }
    #endregion Overrides and Interface
}