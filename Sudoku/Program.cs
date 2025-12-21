
// Set the grid width and height

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Sudoku;



class Program
{

    public static void Main()
    {
        // Create an empty Sudoku Puzzle
        Puzzle puzzle = new();

        // Open the file, read the content, and close it
        // string path = "./SampleInvalidSeed.csv";
        string path = "./SamplePuzzleSeed.csv";
        // string path = "./SamplePuzzleSeed2.csv";
        // string path = "./SamplePuzzleSeed3.csv";
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
        List<int[,]> DistinctSolutions = new();
        Console.WriteLine("Checking Distinct Solutions");
        foreach(var solution in solutions)
        {
            bool exists = false;
            foreach(var otherSolution in DistinctSolutions)
            {
                // If the lengths don't match, they clearly aren't the same.
                if (solution.Length != otherSolution.Length)
                {
                    exists = false;
                    continue;
                }

                bool match = false;
                foreach (var value in solution)
                {
                    foreach (var otherValue in otherSolution)
                    {
                        match = value == otherValue;
                        if (!match)
                        {
                            exists = false;
                            break;
                        }
                    }
                }
                if (match)
                {
                    exists = true;
                }
            }

            if (!exists)
            {
                DistinctSolutions.Add(solution);
            }
        }
        Console.WriteLine($"Found {DistinctSolutions.Count} distinct solutions");

        Console.WriteLine("\n=================================\n");
        Console.WriteLine("Original Puzzle:");
        AsciiGrid asciiPuzzleGrid = new(grid);
        Console.WriteLine(asciiPuzzleGrid);

        Console.WriteLine("\n=================================\n");
        Console.WriteLine("Solutions:");
        foreach (var item in solutions)
        {
            AsciiGrid asciiGrid = new(item);
            Console.WriteLine(asciiGrid);
        }

        Console.WriteLine("\n=================================\n");
        Console.WriteLine("Distinct Solutions:");
        foreach (var item in DistinctSolutions)
        {
            AsciiGrid asciiGrid = new(item);
            Console.WriteLine(asciiGrid);
        }
    }
}