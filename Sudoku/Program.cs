
// Set the grid width and height

using System;
using System.Collections.Generic;
using System.IO;
using Sudoku;



class Program
{
    const int CELL_COUNT = 81;
    const int WIDTH = 9;
    const int HEIGHT = 9;

    public static void Main()
    {
        // Create an empty Sudoku Puzzle
        Puzzle puzzle = new();

        // Open the file, read the content, and close it
        // string path = "./SampleInvalidSeed.csv";
        string path = "./SamplePuzzleSeed.csv";
        // string path = "./SamplePuzzleSeed2.csv";
        string fileContent = File.ReadAllText(path);

        // Print the file content for testing purposes

        // Split the content by lines
        string[] lines = fileContent.Split("\n");


        // Contains the values for each cell in the puzzle. 0 indicates cell is empty.
        int[] puzzleSeed = new int[CELL_COUNT];
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

        puzzle.RevealClues();

        int[][] grid = new int[9][];

        for (int y = 0; y < 9; y++)
        {
            grid[y] = new int[9];
            for (int x = 0; x < 9; x++)
            {
                grid[y][x] = puzzle.GetValue(x, y);
            }
        }

        Console.WriteLine("Original Puzzle:");
        AsciiGrid asciiPuzzleGrid = new(grid);
        Console.WriteLine(asciiPuzzleGrid);
        
        List<int[][]> solutions = new();

        Backtracker.Backtrack(grid, grid, solutions);

        Console.WriteLine($"Found {solutions.Count} solutions");
        // foreach (var solution in solutions)
        // {
        //     asciiPuzzleGrid = new(solution);
        //     Console.WriteLine(asciiPuzzleGrid);
        // }

        List<int[][]> distinctSolutions = GetDistinct2DArrays(solutions);
        
        Console.WriteLine($"Found {distinctSolutions.Count} distinct solutions");
        foreach (var solution in distinctSolutions)
        {
            asciiPuzzleGrid = new(solution);
            Console.WriteLine(asciiPuzzleGrid);
        }
    }

    private static List<int[][]> GetDistinct2DArrays(List<int[][]> arrayList)
    {
        List<int[][]> distinctArrays = new();
        // foreach(var array1 in arrayList)
        for (int i = 0; i < arrayList.Count; i ++)
        {
            var array1 = arrayList[i];
            bool exists = false;
            // foreach(var array2 in distinctArrays)
            for (int j = 0; j < distinctArrays.Count; j++)
            {
                // Console.WriteLine($"Comparing arrayList[{i}] to distinctArrays[{j}]");
                var array2 = distinctArrays[j];
                // If the lengths don't match, they clearly aren't the same.
                if (array1.Length != array2.Length)
                {
                    exists = false;
                    continue;
                }

                bool match = false;
                for (int y = 0; y < HEIGHT; y++)
                {
                    for (int x = 0; x < WIDTH; x++)
                    {
                        match = array1[y][x] == array2[y][x];
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
                distinctArrays.Add(array1);
            }
        }

        return distinctArrays;
    }
}