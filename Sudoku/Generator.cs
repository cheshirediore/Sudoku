using System.Collections.Generic;

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
        bool success = ReverseErode(InitialGrid, out Grid? newPuzzle);
        System.Console.WriteLine($"[Generator.Erosion()] success = {success}");
        return newPuzzle;
    }

    private bool Erode(Grid puzzle, out Grid? newPuzzle)
    {
        Grid candidate = puzzle.ShallowCopy();

        // For each clue value, try removing it and see if it's still a valid puzzle
        for (int index = Grid.SIZE - 1; index > 0; index--)
        {
            if (candidate.GetVertex(index) < 0)
            {
                System.Console.WriteLine($"[Generator.Erode()]Clearing cell #{index}");
                candidate.SetVertex(index, 0);
                Solver solver = new Backtracker(candidate);
                solver.MaxSolutions = 2;
                // If the puzzle is valid, add it to the list
                if (solver.Solve().Count != 1)
                {
                    // Reset the clue
                    System.Console.WriteLine();
                    candidate.SetVertex(index, puzzle.GetVertex(index));
                }
                else
                {
                    System.Console.WriteLine($"[Generator.Erode()] Successfully cleared a clue...");
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

    private bool ReverseErode(Grid puzzle, out Grid? newPuzzle)
    {
        Grid candidate = puzzle.ShallowCopy();

        // For each clue value, try removing it and see if it's still a valid puzzle
        for (int index = 0; index < Grid.SIZE; index++)
        {
            if (candidate.GetVertex(index) < 0)
            {
                System.Console.WriteLine($"[Generator.Erode()]Clearing cell #{index}");
                candidate.SetVertex(index, 0);
                Solver solver = new Backtracker(candidate);
                solver.MaxSolutions = 2;
                // If the puzzle is valid, add it to the list
                if (solver.Solve().Count != 1)
                {
                    // Reset the clue
                    System.Console.WriteLine();
                    candidate.SetVertex(index, puzzle.GetVertex(index));
                }
                else
                {
                    System.Console.WriteLine($"[Generator.Erode()] Successfully cleared a clue...");
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