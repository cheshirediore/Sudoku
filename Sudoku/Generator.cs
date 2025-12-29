using System;
using System.Collections.Generic;

namespace Sudoku;

public class Generator
{
    public const int TargetSeedAmount = 10;
    private readonly Random random;
    public Grid Puzzle {get; private set;}

    /// <summary>
    /// Primary constructor for Generator objects.
    /// </summary>
    /// <param name="randomseed">
    /// An optional seed value for the Random object. By default, no seed is provided. The parameter exists for testing purposes.
    /// </param>
    public Generator(int? randomseed=null)
    {
        Puzzle = new Grid();
        random = randomseed != null? new((int)randomseed): new();
    }

    /// <summary>
    /// Fills an empty Grid object with consistent values, then removes values while ensuring the puzzle is still valid.
    /// </summary>
    /// <returns>
    /// Returns true if and only if a valid puzzle is successfully generated.
    /// </returns>
    public bool Generate()
    {
        bool success = false;
        // Keep trying until the grid is successfully populated
        while (!success)
        {
            success = StochasticFill();
        }
        return Erode() && success;
    }

    /// <summary>
    /// Updates the <c>Puzzle</c> property by removing values that do not result in an invalid puzzle.
    /// </summary>
    /// <returns>
    /// Returns true if and only if the result is different than the starting puzzle.
    /// </returns>
    private bool Erode()
    {
        Grid candidate = Puzzle.ShallowCopy();

        int[] clueIndices = GetRandomIndices();
        
        // For each clue value, try removing it and see if it's still a valid puzzle
        foreach (int index in clueIndices)
        {
            if (candidate.GetVertex(index) < 0)
            {
                candidate.SetVertex(index, 0);
                Solver solver = new Backtracker(candidate)
                {
                    MaxSolutions = 2
                };
                // If the puzzle is valid, add it to the list
                if (solver.Solve().Count != 1)
                {
                    // Reset the clue
                    candidate.SetVertex(index, Puzzle.GetVertex(index));
                }
            }
        }

        bool success = candidate != Puzzle;

        if (success)
        {
            Puzzle = candidate;
            // Set all cells as clues
            for (int index = 0; index < Grid.SIZE; index++)
            {
                Puzzle.SetVertex(index, Puzzle.GetVertex(index), true);
            }
        }
        return success;
    }

    /// <summary>
    /// Updates the <c>Puzzle</c> property by seeding it with random values, and then solving it.
    /// </summary>
    /// <returns>
    /// Returns true if and only if a solution was found.
    /// </returns>
    private bool StochasticFill()
    {
        Grid candidate = Puzzle.ShallowCopy();

        int[] randomIndices = GetRandomIndices();

        candidate.SetVertex(0, random.Next(1, 10), true);
        int populatedCells = 1;

        foreach (int index in randomIndices)
        {
            // Set cell value to a random value
            candidate.SetVertex(index, random.Next(1, 10), true);

            // Check consistency. If consistent, increment populated cell count. Otherwise, restore the original value.
            if (candidate.IsGridConsistent())
            {
                populatedCells++;
            }
            else
            {
                candidate.SetVertex(index, Puzzle.GetVertex(index), true);
            }

            // If populated cell count is at least the targeted amount, break loop
            if (populatedCells >= TargetSeedAmount)
            {
                break;
            }
        }

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
            Puzzle = solutions[0];
            // Set all cells as clues
            for (int index = 0; index < Grid.SIZE; index++)
            {
                Puzzle.SetVertex(index, Puzzle.GetVertex(index), true);
            }
        }

        return success;
    }

    /// <summary>
    /// Uses this instance's <c>random</c> property to randomize the order of cell indices. This is used
    /// to provide random access to the grid vertices without repeated values.
    /// </summary>
    /// <returns>
    /// An array of cell indices for the <c>Puzzle</c>'s vertices in a stochastic order.
    /// </returns>
    private int[] GetRandomIndices()
    {
        int[] vertIndices = new int[Grid.SIZE];

        for (int i = 0; i < Grid.SIZE; i++)
        {
            vertIndices[i] = i;
        }

        Span<int> clueSpan = new Span<int>(vertIndices);

        random.Shuffle<int>(clueSpan);

        return clueSpan.ToArray();
    }

}