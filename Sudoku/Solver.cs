using System.Collections.Generic;

namespace Sudoku;

public class Solver
{
    public Grid Puzzle;

    public Solver(Grid grid)
    {
        Puzzle = grid;
    }

    public List<int[][]> Solve()
    {
        // Solve the sudoku puzzle
        List<int[][]> solutions = new();
        // Backtracker.Backtrack(grid.Vertices, grid.Vertices, solutions); 
        return solutions;
    }
}