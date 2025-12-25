using System;

namespace Sudoku;

public class Generator
{
    public Grid InitialGrid {get; init;}
    public Generator(Grid grid)
    {
        InitialGrid = grid;
    }

    public Grid? Erosion()
    {
        bool success = Erode(InitialGrid, out Grid? newPuzzle);
        System.Console.WriteLine($"[Generator.Erosion()] success = {success}");
        return newPuzzle;
    }

    private bool Erode(Grid puzzle, out Grid? newPuzzle)
    {
        Grid candidate = puzzle.ShallowCopy();
        Random rand = new();
        int[] clueIndices = new int[Grid.SIZE];

        for (int i = 0; i < Grid.SIZE; i++)
        {
            clueIndices[i] = i;
        }

        Span<int> clueSpan = new Span<int>(clueIndices, 0, clueIndices.Length);

        rand.Shuffle<int>(clueSpan);

        clueIndices = clueSpan.ToArray();
        
        // For each clue value, try removing it and see if it's still a valid puzzle
        foreach (int index in clueIndices)
        {
            if (candidate.GetVertex(index) < 0)
            {
                Console.WriteLine($"[Generator.Erode()]Clearing cell #{index}");
                candidate.SetVertex(index, 0);
                Solver solver = new Backtracker(candidate);
                solver.MaxSolutions = 2;
                // If the puzzle is valid, add it to the list
                if (solver.Solve().Count != 1)
                {
                    // Reset the clue
                    Console.WriteLine();
                    candidate.SetVertex(index, puzzle.GetVertex(index));
                }
                else
                {
                    Console.WriteLine($"[Generator.Erode()] Successfully cleared a clue...");
                } 
            }
        }

        bool success = candidate != puzzle;

        if (success)
        {
            newPuzzle = candidate;
        } else
        {
            newPuzzle = null;
        }


        return success;
    }
}