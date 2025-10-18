using System.Text;

public class AsciiGrid
{
    public int sizeX;
    public int sizeY;
    public string fillerCharacter;
    public string[,] grid;

    public AsciiGrid(int width, int height, string filler)
    {
        // Set attributes
        this.sizeX = width;
        this.sizeY = height;
        this.fillerCharacter = filler;

        // Initialize default grid
        this.grid = new string[this.sizeX, this.sizeY];
        // Fill all cells with whitespace to enforce grid structure
        this.Reset();
    }

    public AsciiGrid (int width, int height) : this(width, height, "*")
    {
    }

    public string GetGridCell(int x, int y)
    {
        return this.grid[x, y];
    }

    public void SetGridCell(int x, int y, string newValue)
    {
        if (x >= this.sizeX)
        {
            throw new InvalidOperationException($"y value \"{x}\"exceeds ascii grid width \"{this.sizeX}\".");
        }

        if (y >= this.sizeY)
        {
            throw new InvalidOperationException($"y value \"{y}\"exceeds ascii grid height \"{this.sizeY}\".");
        }

        this.grid[x, y] = newValue;
    }

    public void AddFrame(string frameCharacter)
    {
        for (int y = 0; y < this.sizeY; y++)
        {
            // Set the leftmost column
            this.grid[0, y] = frameCharacter;

            // Set the rightmost column
            this.grid[this.sizeX - 1, y] = frameCharacter;
        }

        for (int x = 0; x < this.sizeX; x++)
        {
            // Set the top row
            this.grid[x, 0] = frameCharacter;

            // Set the bottom row
            this.grid[x, this.sizeY - 1] = frameCharacter;
        }
    }

    public void Reset()
    {
        this.SetAll(" ");
    }

    public void SetAll(string character)
    {
        for (int x = 0; x < this.sizeX; x++)
        {
            for (int y = 0; y < this.sizeY; y++)
            {
                this.grid[x, y] = character;
            }
        }
    }

    public void SetAll()
    {
        this.SetAll(this.fillerCharacter);
    }

    public override string ToString()
    {
        /*
         *    Format the 2D array as an ASCII drawing
         */

        StringBuilder builder = new();
        for (int y = 0; y < this.sizeY; y++) // for each row
        {
            for (int x = 0; x < this.sizeX; x++) // for each column
            {
                builder.Append($"{this.grid[x, y]}");
            }
            builder.AppendLine();
        }

        return builder.ToString();

    }
}