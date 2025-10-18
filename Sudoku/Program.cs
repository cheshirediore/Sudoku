
// Set the grid width and height
using System.Text;
using Sudoku;

int Width, Height;
Width = 9;
Height = 9;

// Create a Sudoku Puzzle
Puzzle puzzle = new(Width, Height);

// Create the AsciiGrid to render the Sudoku puzzle
AsciiGrid asciiGrid = new(Width, Height);
// asciiGrid.SetAll();

for (int x = 0; x < Width; x++)
{
    for (int y = 0; y < Height; y++)
    {
        string cellValue = $"{puzzle.GetCell(x, y).Value}";

        asciiGrid.SetGridCell(x, y, cellValue);
    }
}
Console.WriteLine(asciiGrid);