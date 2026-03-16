using System.Collections.Generic;

namespace Sudoku;

public abstract class Solver
{
    // A reference to the original puzzle the object is dedicated to solving.
    public abstract Puzzle SudokuPuzzle {get; init;}
    public abstract int MaxSolutions {get; set;}

    // A method to return one or more solution(s) to the puzzle
    public abstract List<Puzzle> Solve();
}