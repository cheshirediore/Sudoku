using System;
using System.Collections.Generic;
using Sudoku.Utility;

namespace Sudoku;

class Program
{
    const int WIDTH = 9;
    const int HEIGHT = 9;

    public static void Main(string[] args)
    {
        // RunTests();
        string nakedPairTestSeedFilePath = System.IO.Path.GetFullPath("resources/NakedPairSeed.csv");
        Puzzle puzzle = Importer.PuzzleFromCSV(nakedPairTestSeedFilePath);
        SolvePuzzle(puzzle);
        // TechniqueSolver solver = new(puzzle);
        // Console.WriteLine(puzzle);
        // List<Action> actions = solver.NakedPair();
        // foreach (var action in actions)
        // {
        //     Console.WriteLine(action);
        // }
        // Console.WriteLine(puzzle.CellGrid.GetVertex(54));
        // Console.WriteLine(puzzle.CellGrid.GetVertex(55));
        // Console.WriteLine(puzzle.CellGrid.GetVertex(56));
    }

    public static void RunTests()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        TestGenerator();
        watch.Stop();
        Console.WriteLine($"Generator.Generate() runtime: {watch.Elapsed.TotalSeconds} seconds");
    }

    private static void SolvePuzzle(Puzzle grid)
    {
        // Create a Solver to process the imported seed file
        Solver solver = new Backtracker(grid)
        {
            MaxSolutions = 2
        };
        // Solve the sudoku puzzle
        List<Puzzle> solutions = solver.Solve();

        // Distill the list of solutions to filter out the duplicates
        // Ideally, this step is redundant. However, if there is something wrong in the solving process,
        // then this will highlight a discrepancy.
        HashSet<Puzzle> distinctSolutions = [..solutions];

        // Output the results
        System.Console.WriteLine("Original Puzzle:");
        System.Console.WriteLine(grid);

        if (distinctSolutions.Count > 1)
        {
            System.Console.WriteLine($"Found {solutions.Count} solutions");
            System.Console.WriteLine("Puzzle is invalid.");
        }
        else if (solutions.Count > 0)
        {
            System.Console.WriteLine("Solution found!");
            System.Console.WriteLine(solutions[0]);
        } 
        else
        {
            System.Console.WriteLine("No solutions found.");
        }
    }

    private static void TestGenerator()
    {
        Generator generator = new();
        generator.Generate();
        System.Console.WriteLine(generator.SudokuPuzzle);
    }

    
    private static string GetAsciiReprGrid(int[][] grid)
    {
        return new Sudoku.Utility.AsciiGrid(grid).ToString();
    }

}