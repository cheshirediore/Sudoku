using System;
using System.Text;

/// <summary>
/// Class <c>AsciiGrid</c> is a scaffolding class that renders 2D array values to the terminal.
/// It is used in this project for testing and debugging in lieu of a polished or prototype GUI.
/// </summary>
public class AsciiGrid
{
    public int Width;
    public int Height;
    public string fillerCharacter;
    public string[][] grid;

    private static string HorizontalSeparator = "+-------+-------+-------+";

    public string LinePrefix = "";

    public AsciiGrid(int width, int height, string filler)
    {
        // Set attributes
        Width = width;
        Height = height;
        fillerCharacter = filler;

        // Initialize default grid
        grid = new string[Height][];
        for (int y = 0; y < Height; y++)
        {
            grid[y] = new string[Width];
        }
        // Fill all cells with whitespace to enforce grid structure
        Reset();
    }

    public AsciiGrid(int width, int height) : this(width, height, "*")
    {
    }

    public AsciiGrid(int[][] integerGrid)
    {
        Height = integerGrid.Length;
        Width = integerGrid[0].Length;
        fillerCharacter = "*";
        grid = new string[Height][];
        for (int y = 0; y < Width; y++)
        {
            grid[y] = new string[Width];
        }

        // Nested for loops - row by row access
        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                grid[row][col] = Math.Abs(integerGrid[row][col]).ToString();
            }
        }
    }

    public AsciiGrid(int[][] integerGrid, int indentationSize) : this(integerGrid)
    {
        for (int i = 0; i < indentationSize; i++)
        {
            LinePrefix = $"{LinePrefix}    ";
        }
    }

    public string GetGridCell(int x, int y)
    {
        return grid[y][x];
    }

    public void SetGridCell(int x, int y, string newValue)
    {
        if (x >= Width)
        {
            throw new System.InvalidOperationException($"y value \"{x}\"exceeds ascii grid width \"{Width}\".");
        }

        if (y >= Height)
        {
            throw new System.InvalidOperationException($"y value \"{y}\"exceeds ascii grid height \"{Height}\".");
        }

        grid[y][x] = newValue;
    }

    public void AddFrame(string frameCharacter)
    {
        for (int x = 0; x < Height; x++)
        {
            // Set the leftmost column
            grid[0][x] = frameCharacter;

            // Set the rightmost column
            grid[Width - 1][x] = frameCharacter;
        }

        for (int y = 0; y < Width; y++)
        {
            // Set the top row
            grid[y][0] = frameCharacter;

            // Set the bottom row
            grid[y][Height - 1] = frameCharacter;
        }
    }

    public void Reset()
    {
        SetAll(" ");
    }

    public void SetAll(string character)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                grid[y][x] = character;
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

        // Add the line prefix
        builder.Append(LinePrefix);
        // Top of the frame
        builder.AppendLine(HorizontalSeparator);

        // Althought we normally access the cells column by column,
        // we print them row by row
        for (int y = 0; y < Height; y++)
        {
            // Add a horizontal separator before the first cell of the 4th and 7th rows
            if (y == 3 || y == 6)
            {
                // Add the line prefix
                builder.Append(LinePrefix);
                builder.AppendLine(HorizontalSeparator);
            }

            // Add the line prefix
            builder.Append(LinePrefix);
            
            // Add the left side of the frame at the start of every row
            builder.Append('|');
            for (int x = 0; x < Width; x++)
            {

                // Add a vertical separator before the 1st, 4th, and 7th columns
                if (x == 3 || x == 6)
                {
                    builder.Append(" | ");
                }
                else
                {
                    builder.Append(' ');
                }

                // Add the ascii cell value itself
                string cellValue = GetGridCell(x, y);
                if (cellValue == "0")
                {
                    cellValue = " ";
                }
                builder.Append(cellValue);

            }
            // Add a vertical separator after the last column to close the frame and move on to the next row
            builder.AppendLine(" |");
        }

        // Bottom of the frame
        // Add the line prefix
        builder.Append(LinePrefix);
        builder.AppendLine(HorizontalSeparator);

        return builder.ToString();
    }
}