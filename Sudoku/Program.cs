
// Set the grid width and height
using System.Text;
using Sudoku;

int Width, Height;
Width = 9;
Height = 9;

// Create a Sudoku Puzzle
Puzzle puzzle = new();

int i = 0;
for (int x = 0; x < Width; x++)
{
    for (int y = 0; y < Height; y++)
    {
        puzzle.SetCellValue(x, y, i);
        i++;
    }
}

Console.WriteLine("------");

// Test GetRow
StringBuilder builder = new();
Cell[] row = puzzle.GetRow(3);
for (i = 0; i < row.Length; i++)
{
    builder.Append($"{row[i].Value} ");
}
Console.WriteLine(builder);
Console.WriteLine("------");

// Test GetColumn
builder.Clear();
Cell[] column = puzzle.GetColumn(3);
for (i = 0; i < column.Length; i++)
{
    builder.Append($"{column[i].Value} ");
}
Console.WriteLine(builder);
Console.WriteLine("------");

// Test GetBlock
builder.Clear();
Cell[] block = puzzle.GetBlock(8);
for (i = 0; i < block.Length; i++)
{
    builder.Append($"{block[i].Value} ");
}
Console.WriteLine(builder);
Console.WriteLine("------");

// Create the AsciiGrid to render the Sudoku puzzle
AsciiGrid asciiGrid = new(Width, Height);
// asciiGrid.SetAll();

for (int x = 0; x < Width; x++)
{
    for (int y = 0; y < Height; y++)
    {
        string cellValue = $"{puzzle.GetCellValue(x, y)}";

        asciiGrid.SetGridCell(x, y, cellValue);
    }
}
Console.WriteLine(asciiGrid);