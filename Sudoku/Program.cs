
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
        // int[][] grid = ImportSeedFile(path);
        Grid grid = new(path);

        // Solve the sudoku puzzle
        List<int[][]> solutions = new();
        Backtracker.Backtrack(grid.Vertices, grid.Vertices, solutions);

        // Distill the list of solutions to filter out the duplicates (ideally, this is redundant; it has not been proven for this process)
        List<int[][]> distinctSolutions = GetDistinct2DArrays(solutions);
        
        // Output the results
        
        Console.WriteLine("Original Puzzle:");
        Console.WriteLine(GetAsciiReprGrid(grid.Vertices));
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

    private static int[][] ImportSeedFile(string path)
    {
        // Open the file, read the content, and close it
        string fileContent = File.ReadAllText(path);

        // Split the content by lines
        string[] lines = fileContent.Split("\n");
        if (lines.Length != HEIGHT)
        {
            throw new ArgumentOutOfRangeException(path, $"Input puzzle seed must have {HEIGHT} lines. Provided seed has '{lines.Length}'.");
        }
        // Initialize grid
        int[][] grid = new int[HEIGHT][];

        // Iterate over the lines and add the values to the grid
        for (int y = 0; y < HEIGHT; y++)
        {
            // Create the row in the grid
            grid[y] = new int[WIDTH];

            // Split the line by commas, and trim off the whitespace
            string[] rowValues = lines[y].Split(",");
            // Verify that the width is correct before adding it to the grid
            if (rowValues.Length != WIDTH)
            {
                throw new ArgumentOutOfRangeException(path, $"'{rowValues.Length}' is an invalid width. All rows in the input puzzle seed must have a width of {WIDTH}.");
            }
            // Verify that each string is numeric, and add it to the grid if it is. Otherwise, throw an exception. TODO: change line 104 into a verbose IF/ELSE block with tha exception
            for (int x = 0; x < rowValues.Length; x++)
            {
                // bool success = int.TryParse(rowValues[x].Trim(), out int parsedValue);
                // grid[y][x] = success ? parsedValue * -1 : 0; 
                if (int.TryParse(rowValues[x].Trim(), out int parsedValue))
                {
                    // Using negative numbers to flag the clue values using a single int
                    grid[y][x] = parsedValue * -1;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(path, $"Invalid value passed in puzzle seed. Check file for non-numeric characters.");
                }
            }
        }
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