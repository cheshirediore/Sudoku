
// Set the grid width and height
using System.Text;
using Sudoku;

int Width, Height;
Width = 9;
Height = 9;

// Create a Sudoku Puzzle
Puzzle puzzle = new();

int[] PuzzleSolution = [
    8, 7, 5, 9, 2, 1, 3, 4, 6,
    3, 6, 1, 7, 5, 4, 8, 9, 2,
    2, 4, 9, 8, 6, 3, 7, 1, 5,

    5, 8, 4, 6, 9, 7, 1, 2, 3,
    7, 1, 3, 2, 4, 8, 6, 5, 9,
    9, 2, 6, 1, 3, 5, 4, 8, 7,

    6, 9, 7, 4, 1, 2, 5, 3, 8,
    1, 5, 8, 3, 7, 9, 2, 6, 4,
    4, 3, 2, 5, 8, 6, 9, 7, 1
];

int[][] RevealedCoords = [
    [1, 0], [4, 0], [7, 0], [8, 0],
    [1, 1], [6, 1], [7, 1],
    [0, 2], [3, 2], [6, 2], [7, 2], [8, 2],
    [1, 3], [2, 3], [4, 3], [5, 3],
    [0, 4], [1, 4], [7, 4], [8, 4],
    [3, 5], [4, 5], [6, 5], [7, 5],
    [0, 6], [1, 6], [2, 6], [5, 6], [8, 6],
    [1, 7], [2, 7], [7, 7],
    [0, 8], [1, 8], [4, 8], [7, 8]
];

int i = 0;
for (int x = 0; x < Width; x++)
{
    for (int y = 0; y < Height; y++)
    {
        puzzle.SetValue(x, y, PuzzleSolution[i]);
        i++;
    }
}

for (i = 0; i < RevealedCoords.Length; i++)
{
    int x, y;
    x = RevealedCoords[i][0];
    y = RevealedCoords[i][1];
    puzzle.SetPlayerValue(x, y, puzzle.GetValue(x, y));
}


// Create the AsciiGrid to render the Sudoku puzzle
AsciiGrid asciiGrid = new(Width, Height);

for (int x = 0; x < Width; x++)
{
    for (int y = 0; y < Height; y++)
    {
        string cellValue = $"{puzzle.GetPlayerValue(x, y)}";

        asciiGrid.SetGridCell(x, y, cellValue);
    }
}
Console.WriteLine(asciiGrid);

puzzle.ValidateSolution();
Console.WriteLine(puzzle.IsSolved);
puzzle.SetPlayerValue(8, 8, 8);
puzzle.ValidateSolution();
Console.WriteLine(puzzle.IsSolved);



for (i = 0; i < 81; i++)
{
    Console.WriteLine($"{i} = ({Puzzle.GetCellCoordinatesByIndex(i)[0]}, {Puzzle.GetCellCoordinatesByIndex(i)[1]})");
}

