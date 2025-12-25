using System;
using System.Collections.Generic;

namespace Sudoku;

public class Generator
{
    public Grid InitialGrid {get; set;}

    public Generator(Grid grid)
    {
        InitialGrid = grid;
    }

    public Generator(): this(new Grid()) {}

    public bool Erode()
    {
        Grid candidate = InitialGrid.ShallowCopy();

        int[] clueIndices = GetRandomIndices();
        // Console.WriteLine($"clueIndices.Length = {clueIndices.Length}");
        
        // For each clue value, try removing it and see if it's still a valid puzzle
        foreach (int index in clueIndices)
        {
            // Console.WriteLine($"[Generator.Erode()] Processing #{index}");

            if (candidate.GetVertex(index) < 0)
            {
                // Console.WriteLine($"[Generator.Erode()] Clearing cell #{index}");
                candidate.SetVertex(index, 0);
                Solver solver = new Backtracker(candidate)
                {
                    MaxSolutions = 2
                };
                // If the puzzle is valid, add it to the list
                if (solver.Solve().Count != 1)
                {
                    // Console.WriteLine($"[Generator.Erode()] Failed to clear cell {index}. Causes invalid puzzle.");
                    // Reset the clue
                    candidate.SetVertex(index, InitialGrid.GetVertex(index));
                }
                // else
                // {
                //     Console.WriteLine($"[Generator.Erode()] Successfully cleared cell {index}");
                // } 
            }
        }

        bool success = candidate != InitialGrid;

        if (success)
        {
            InitialGrid = candidate;
            // Set all cells as clues
            for (int index = 0; index < Grid.SIZE; index++)
            {
                InitialGrid.SetVertex(index, InitialGrid.GetVertex(index), true);
            }
        }

        return success;
    }

    public bool Fill()
    {
        Random rand = new();
        Grid candidate = InitialGrid.ShallowCopy();

        int targetSeedAmount = 10;
        int[] randomIndices = GetRandomIndices();

        candidate.SetVertex(0, rand.Next(1, 10), true);
        int populatedCells = 1;

        foreach (int index in randomIndices)
        {
            // Set cell value to a random value
            candidate.SetVertex(index, rand.Next(10), true);

            // Check consistency. If consistent, increment populated cell count. Otherwise, restore the original value.
            if (candidate.IsConsistent())
            {
                populatedCells++;
            }
            else
            {
                candidate.SetVertex(index, InitialGrid.GetVertex(index), true);
            }

            // If populated cell count is at least the targeted amount, break loop
            if (populatedCells >= targetSeedAmount)
            {
                break;
            }
        }

        Console.WriteLine($"[Generator.Fill()] Seed Values:");
        Console.WriteLine($"{new AsciiGrid(candidate.Vertices)}");

        // Solve the puzzle
        // Create a Solver to process the grid
        Solver solver = new Backtracker(candidate)
        {
            MaxSolutions = 1
        };


        // Solve the sudoku puzzle
        List<Grid> solutions = solver.Solve();

        bool success = solutions.Count >= 1;

        if (success)
        {
            InitialGrid = solutions[0];
            // Set all cells as clues
            for (int index = 0; index < Grid.SIZE; index++)
            {
                InitialGrid.SetVertex(index, InitialGrid.GetVertex(index), true);
            }
        }
        else
        {
            Console.WriteLine("Failed to generate a valid puzzle.");
        }

        return success;
    }

    private static int[] GetRandomIndices()
    {
        Random rand = new();
        int[] vertIndices = new int[Grid.SIZE];

        for (int i = 0; i < Grid.SIZE; i++)
        {
            vertIndices[i] = i;
        }

        Span<int> clueSpan = new Span<int>(vertIndices);

        rand.Shuffle<int>(clueSpan);

        return clueSpan.ToArray();
        // return rand.GetItems<int>(vertIndices, Grid.SIZE);
    }

}