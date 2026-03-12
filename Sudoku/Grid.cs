namespace Sudoku;

public class Grid<T>
{
    // Static Constants //
    // Only a 9x9 grid is supported by this implementation. WIDTH, HEIGHT, and SIZE are provided for clean reference by other objects.
    public const int WIDTH = 9;
    public const int HEIGHT = 9;
    public const int REGION_COUNT = 9;
    public static int SIZE { get => WIDTH * HEIGHT; }

    // Instance Attributes //
    public List<T> Vertices
    {
        get
        {
            lock (_lock)
            {
                return _vertices.AsReadOnly();
            }
        }
    } 
    private List<T> _vertices;

    public Grid()
    {
        _vertices = new List<T>();
        for (int i = 0; i < SIZE; i++)
        {
            _vertices.Add(new T());
        }
    }

    #region Static Methods
    /// <summary>
    /// Converts a pair of coordinates to a cell index.
    /// </summary>
    /// <param name="coordinates">
    /// A Cartesian coordinate pair indicating the location of the cell on a Cartesian plane.
    /// </param>
    /// <returns>
    /// An integer value indicating the number of the cell as counted from top left to bottom right of the grid.
    /// </returns>
    /// <remarks>
    /// Does not check if the given coordinates (and corresponding index) are valid for a Grid.
    /// </remarks>
    private static int CoordinatesToIndex(int[] coordinates)
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
    /// The Cartesian coordinate pair of the cell indicated by the <paramref name="index"/>.
    /// </returns>
    /// <remarks>
    /// Does not check if the given index (and corresponding pair of coordinates) is valid for a Grid.
    /// </remarks>
    private static int[] IndexToCoordinates(int index)
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
    /// Returns index indicating which block contains the given pair of coordinates.
    /// If the pair of coordinates is invalid, returns a -1.
    /// </returns>
    private static int GetBlockIndex(int x, int y)
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
    private static int GetBlockIndex(int cellIndex)
    {
        int[] coordinates = IndexToCoordinates(cellIndex);  
        return GetBlockIndex(coordinates[0], coordinates[1]);   
    }
    #endregion

    #region GetVertex
    /// <summary>
    /// Primary accessor method for getting the value of a given vertex.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="index"/> is less than 0 or greater than SIZE.
    /// </exception>
    public T GetVertex(int index)
    {
        // int[] coords = IndexToCoordinates(index);
        // return GetVertex(coords[0], coords[1]);
        if (index < 0 || index > SIZE)
        {
            throw new System.ArgumentOutOfRangeException(nameof(index), $"{index} is outside of the grid bounds.");
        }
        return Vertices[index];
    }

    public T GetVertex(int x, int y)
    {
        int index = CoordinatesToIndex(x, y);
        return GetVertex(index);
    }
    #endregion

}