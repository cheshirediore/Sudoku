using System.Text;

/// <summary>
/// Class <c>AsciiGrid</c> is a scaffolding class that renders 2D array values to the terminal.
/// It is used in this project for testing and debugging in lieu of a polished or prototype GUI.
/// </summary>
public class AsciiGrid
{
    public int sizeX;
    public int sizeY;
    public string fillerCharacter;
    public string[,] grid;

    public AsciiGrid(int width, int height, string filler)
    {
        // Set attributes
        sizeX = width;
        sizeY = height;
        fillerCharacter = filler;

        // Initialize default grid
        grid = new string[sizeX, sizeY];
        // Fill all cells with whitespace to enforce grid structure
        Reset();
    }

    public AsciiGrid(int width, int height) : this(width, height, "*")
    {
    }

    public string GetGridCell(int x, int y)
    {
        return grid[x, y];
    }

    public void SetGridCell(int x, int y, string newValue)
    {
        if (x >= sizeX)
        {
            throw new InvalidOperationException($"y value \"{x}\"exceeds ascii grid width \"{sizeX}\".");
        }

        if (y >= sizeY)
        {
            throw new InvalidOperationException($"y value \"{y}\"exceeds ascii grid height \"{sizeY}\".");
        }

        grid[x, y] = newValue;
    }

    public void AddFrame(string frameCharacter)
    {
        for (int y = 0; y < sizeY; y++)
        {
            // Set the leftmost column
            grid[0, y] = frameCharacter;

            // Set the rightmost column
            grid[sizeX - 1, y] = frameCharacter;
        }

        for (int x = 0; x < sizeX; x++)
        {
            // Set the top row
            grid[x, 0] = frameCharacter;

            // Set the bottom row
            grid[x, sizeY - 1] = frameCharacter;
        }
    }

    public void Reset()
    {
        SetAll(" ");
    }

    public void SetAll(string character)
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                grid[x, y] = character;
            }
        }
    }

    public void SetAll()
    {
        SetAll(fillerCharacter);
    }

    public override string ToString()
    {
        /*
         *    Format the 2D array as an ASCII drawing
         */

        StringBuilder builder = new();
        for (int y = 0; y < sizeY; y++) // for each row
        {
            for (int x = 0; x < sizeX; x++) // for each column
            {
                builder.Append($"{grid[x, y]}");
                // Add a space unless it's the end of the row
                if (x < sizeX -1)
                {
                    builder.Append(' ');
                }
            }
            builder.AppendLine();
        }

        return builder.ToString();

    }
}