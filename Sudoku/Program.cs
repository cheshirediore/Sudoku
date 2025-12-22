
// Set the grid width and height

using System;
using System.Collections.Generic;
using System.IO;
using Sudoku;



class Program
{
    const int WIDTH = 9;
    const int HEIGHT = 9;

    public static void Main(string[] args)
    {
        string[] seedPaths = [
            "./SamplePuzzleSeed.csv",
            "./SamplePuzzleSeed2.csv",
            "./SampleInvalidSeed.csv"
        ];

        int pathNumber = 0;

        if (args.Length > 0)
        {
            if (int.TryParse(args[0], out pathNumber))
            {
                if (pathNumber >= 0 && pathNumber < seedPaths.Length)
                { 
                    Console.WriteLine($"Using {seedPaths[pathNumber]}");
                } 
                else
                {
                    Console.WriteLine($"Provided argument {pathNumber} invalid. Using {seedPaths[pathNumber]} as path.");
                    pathNumber = 0;
                }
            }
        }

        string path = seedPaths[pathNumber];
        
        // Read the input seed file and generate a sudoku grid array
        int[][] grid = GenerateGrid(path);

        // Solve the sudoku puzzle
        List<int[][]> solutions = new();
        Backtracker.Backtrack(grid, grid, solutions);

        // Distill the list of solutions to filter out the duplicates (ideally, this is redundant; it has not been proven for this process)
        List<int[][]> distinctSolutions = GetDistinct2DArrays(solutions);
        
        // Output the results
        
        Console.WriteLine("Original Puzzle:");
        Console.WriteLine(GetAsciiReprGrid(grid));
        Console.WriteLine($"Found {solutions.Count} solutions");
        // foreach (var solution in solutions)
        // {
        //      Console.WriteLine(GetAsciiReprGrid(solution));
        // }
        Console.WriteLine($"Found {distinctSolutions.Count} distinct solutions");
        // foreach (var solution in distinctSolutions)
        // {
        //     Console.WriteLine(GetAsciiReprGrid(solution));
        // }
        
    }

    private static string GetAsciiReprGrid(int[][] grid)
    {
        return new AsciiGrid(grid).ToString();
    }

    private static int[][] GenerateGrid(string path)
    {
        // Create an empty Sudoku Puzzle
        Puzzle puzzle = new();

        // Open the file, read the content, and close it
        string fileContent = File.ReadAllText(path);

        // Print the file content for testing purposes

        // Split the content by lines
        string[] lines = fileContent.Split("\n");

        // Contains the values for each cell in the puzzle. 0 indicates cell is empty.
        int[] puzzleSeed = new int[WIDTH * HEIGHT];
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
        for (int i = 0; i < WIDTH * HEIGHT; i++)
        {
            puzzle.SetValue(i, puzzleSeed[i]);
            puzzle.SetPlayerValue(i, puzzleSeed[i]);
            if (puzzleSeed[i] != 0)
            {
                puzzle.RegisterClue(i);
            }
        }

        puzzle.RevealClues();

        int[][] grid = new int[HEIGHT][];

        for (int y = 0; y < HEIGHT; y++)
        {
            grid[y] = new int[WIDTH];
            for (int x = 0; x < WIDTH; x++)
            {
                grid[y][x] = puzzle.GetValue(x, y);
            }
        }

        // Console.WriteLine(puzzle);

        return grid;
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