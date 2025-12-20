
// Set the grid width and height

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sudoku;


// Create an empty Sudoku Puzzle
Puzzle puzzle = new();

// Open the file, read the content, and close it
// string path = "./SampleInvalidSeed.csv";
string path = "./SamplePuzzleSeed2.csv";
string fileContent = File.ReadAllText(path);

// Print the file content for testing purposes
// Console.WriteLine(fileContent);

// Split the content by lines
string[] lines = fileContent.Split("\n");

Console.WriteLine("\n=================================\n");

// Contains the values for each cell in the puzzle. 0 indicates cell is empty.
int[] puzzleSeed = new int[81];
int index = 0;
foreach (var line in lines)
{
    // Split the line by commas, and trim off the whitespace
    string[] rowValues = line.Split(",");
    for (int i = 0; i < rowValues.Length; i++)
    {
        bool success = int.TryParse(rowValues[i].Trim(), out int parsedValue);
        puzzleSeed[index] = success ? parsedValue * -1 : 0; // Using negative numbers to flag the clue values using a single int
        index++;
    }
}

// initialize the puzzle grid. This could be combined with the previous loop for efficiency, 
// but this is just for testing anyway. Ultimately, this will probably be an argument for the 
// Puzzle constructor.
for (int i = 0; i < 81; i++)
{
    puzzle.SetValue(i, puzzleSeed[i]);
    puzzle.SetPlayerValue(i, puzzleSeed[i]);
    if (puzzleSeed[i] != 0)
    {
        puzzle.RegisterClue(i);
    }
}

puzzle.PrintClueIndices();

puzzle.RevealClues();
Console.WriteLine("\n=================================\n");

Console.WriteLine(puzzle);

Console.WriteLine("\n=================================\n");

int[,] grid = new int[9, 9];

for (int y = 0; y < 9; y++)
{
    for (int x = 0; x < 9; x++)
    {
        grid[y, x] = puzzle.GetValue(x, y);
    }
}

List<int[,]> solutions = new();

Backtracker.Backtrack(grid, grid, solutions);

Console.WriteLine($"Found {solutions.Count} solutions");

Console.WriteLine("\n=================================\n");

AsciiGrid asciiPuzzleGrid = new(grid);
Console.WriteLine(asciiPuzzleGrid);

Console.WriteLine("\n=================================\n");

foreach (var item in solutions)
{
    AsciiGrid asciiGrid = new(item);
    Console.WriteLine(asciiGrid);
}
// Solver solver = new(puzzle);
// int numberOfSolutionsFound = solver.Solve();

// Console.WriteLine($"Found {numberOfSolutionsFound} solutions");
// Console.WriteLine("\n=================================\n");

/*

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

*/