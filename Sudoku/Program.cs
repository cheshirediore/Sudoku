using System;
using System.Collections.Generic;

namespace Sudoku;

class Program
{
    const int WIDTH = 9;
    const int HEIGHT = 9;

    public static void Main(string[] args)
    {
        TestGenerator();     
    }

    private static string GetAsciiReprGrid(int[][] grid)
    {
        return new AsciiGrid(grid).ToString();
    }

    private static void TestIndexTranslation()
    {
        for (int y = 0; y < Grid.HEIGHT; y++)
        {
            for (int x = 0; x < Grid.WIDTH; x++)
            {
                int[] inputCoordinates = [x, y];
                int index = Grid.CoordinatesToIndex(inputCoordinates);    
                int[] outputCoordinates = Grid.IndexToCoordinates(index);
                Console.WriteLine($"Index: {index}; x={x}, y={y}");
                Console.WriteLine($"Input Coordinates:  [{inputCoordinates[0]}, {inputCoordinates[1]}]");  
                Console.WriteLine($"Output Coordinates: [{outputCoordinates[0]}, {outputCoordinates[1]}]");
            }
        }
    }

    private static void TestSolver(Grid grid)
    {
        // Create a Solver to process the imported seed file
        Solver solver = new Backtracker(grid)
        {
            MaxSolutions = 2
        };
        // Solve the sudoku puzzle
        List<Grid> solutions = solver.Solve();

        // Distill the list of solutions to filter out the duplicates
        // Ideally, this step is redundant. However, if there is something wrong in the solving process,
        // then this will highlight a discrepancy.
        HashSet<Grid> distinctSolutions = [..solutions];

        // Output the results
        System.Console.WriteLine("Original Puzzle:");
        System.Console.WriteLine(GetAsciiReprGrid(grid.Vertices));
        if (distinctSolutions.Count > 1)
        {
            System.Console.WriteLine($"Found {solutions.Count} solutions");
            System.Console.WriteLine("Puzzle is invalid.");
        }
        else if (solutions.Count > 0)
        {
            System.Console.WriteLine("Solution found!");
            System.Console.WriteLine(GetAsciiReprGrid(solutions[0].Vertices));
            System.Console.WriteLine();
            System.Console.WriteLine(solutions[0]);
        }
    }

    private static void TestGenerator()
    {
        Generator generator = new(13);
        generator.Generate();
        System.Console.WriteLine(generator.Puzzle);
        System.Console.WriteLine(GetAsciiReprGrid(generator.Puzzle.Vertices));
    }

    private static Grid ImportSeedFile(string[] args)
    {
        // Hard-Coded paths for testing
        string[] seedPaths = [
            "./SamplePuzzleSeed.csv",
            "./SamplePuzzle.csv",
            "./SampleInvalidSeed.csv",
            "./EmptyPuzzleSeed.csv"
        ];

        // Parse the CLI input and try to select a file path from the above list
        int pathNumber = 0;

        if (args.Length > 0)
        {
            if (int.TryParse(args[0], out pathNumber))
            {
                if (pathNumber >= 0 && pathNumber < seedPaths.Length)
                { 
                    System.Console.WriteLine($"Using {seedPaths[pathNumber]}");
                } 
                else
                {
                    throw new System.ArgumentOutOfRangeException($"{seedPaths[pathNumber]}", $"Provided argument {pathNumber} invalid. Using {seedPaths[pathNumber]} as path.");
                }
            }
        }

        string path = seedPaths[pathNumber];
        
        // Verify that the chosen path actually exists
        if (!System.IO.File.Exists(path))
        {
            throw new System.IO.FileNotFoundException($"File path '{seedPaths[pathNumber]}' not found. Verify the file exists, and that the permissions are correct.");
        }

        // Read the input seed file and generate a sudoku grid array
        return new Grid(path);
    }
}