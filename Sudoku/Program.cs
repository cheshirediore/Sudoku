
// Set the grid width and height

using System.Text;
using Sudoku;

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

for (int i = 0; i < 81; i++)
{
    int[] coords = puzzle.GetCellCoordinatesByIndex(i);
    if (Array.Exists(RevealedCoords, coords.SequenceEqual))
    {
        puzzle.SetValue(coords[0], coords[1], PuzzleSolution[i]);
        puzzle.RevealCell(coords[0], coords[1]);
        // Console.WriteLine($"({i}) Revealing cell ({coords[0]}, {coords[1]}) = {puzzle.GetValue(i)}");
    }
    else
    {
        puzzle.SetValue(coords[0], coords[1], 0);
        // Console.WriteLine($"({i}) Clearing cell ({coords[0]}, {coords[1]})");

    }
}

Console.WriteLine(puzzle);

// Console.WriteLine("=================================================");
// Console.WriteLine("=================================================");
// Console.WriteLine("=================================================");


Solver solver = new(puzzle);
solver.Solve();


for (int i = 0; i < 81; i++)
{
    puzzle.RevealCell(i);
}

Console.WriteLine(puzzle);