
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
asciiGrid.SetAll();
for (int x = 0; x < Width; x++)
{
    for (int y = 0; y < Width; y++)
    {
        StringBuilder cellValue = new();
        // Add a horizontal separator before the first cell of the 1st, 4th, 7th rows
        if ((y == 0 || y == 3 || y == 6) && x == 0)
        {
            cellValue.AppendLine("+-------+-------+-------+");
        }
        // Add a vertical separator before the 1st, 4th, and 7th columns
        if (x == 0 || x == 3 || x == 6)
        {
            cellValue.Append("| ");
        }

        cellValue.Append($"{puzzle.GetCell(x, y).Value}");

        // Add a vertical separator after the last column to close the frame
        if (x == Width - 1)
        {
            cellValue.Append(" |");
        }
        
        if (y == Height - 1 && x == Width - 1)
        {
            cellValue.AppendLine();
            cellValue.Append("+-------+-------+-------+");
        }
        asciiGrid.SetGridCell(x, y, cellValue.ToString());
    }
}
Console.WriteLine(asciiGrid);