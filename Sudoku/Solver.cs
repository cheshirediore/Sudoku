using System.Collections.Generic;

namespace Sudoku;

public class Solver
{
    public Grid Puzzle;

    public Solver(Grid sudokuGrid)
    {
        Puzzle = sudokuGrid;
    }

    public List<int[][]> Solve()
    {
        // Solve the sudoku puzzle using backtracking
        List<int[][]> solutions = Backtracker.Backtrack(Puzzle.Vertices, Puzzle.Vertices, new List<int[][]>()); 
        return solutions;
    }
}