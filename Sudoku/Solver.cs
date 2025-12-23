using System.Collections.Generic;

namespace Sudoku;

public abstract class Solver
{
    // A reference to the original puzzle the object is dedicated to solving.
    public abstract Grid Puzzle {get; init;}

    // A method to return one or more solution(s) to the puzzle
    public abstract List<Grid> Solve();
}