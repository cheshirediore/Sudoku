namespace Sudoku;

/// <summary>
/// Given an incomplete <c>Puzzle</c>, the <c>Solver</c> determines a valid solution (if possible).
/// </summary>
public class Solver
{
    private Puzzle _puzzle;
    private int[] _emptyCellIndices;

    public Solver(Puzzle puzzle)
    {
        _puzzle = puzzle;
        _emptyCellIndices = _puzzle.GetEmptyCellIndices();
    }

    public void Solve()
    {
        // Set the current cell as the first cell
        Backtracker(0);
    }
    
    private bool Backtracker(int currentIndex)
    {
        int nextIndex = currentIndex;
        //Console.WriteLine($"Solver.Backtracker({currentIndex}). Current Cell: {_puzzle.GetCell(_emptyCellIndices[currentIndex])}. Empty Cell Count: {_emptyCellIndices.Length}.");
        // Process each cell, and check the return value
        int newValue = _puzzle.GetValue(_emptyCellIndices[currentIndex]) + 1;
        
        if (newValue <= 9)
        {
            // Increment the cell
           // Console.WriteLine($"Solver.Backtracker({currentIndex}). Incrementing value of {_puzzle.GetCell(_emptyCellIndices[currentIndex])}.");
            _puzzle.SetValue(_emptyCellIndices[currentIndex], newValue);
        }
        // Check if the puzzle is consistent
        bool valid = _puzzle.IsConsistent();

        // If it's valid, move forward. If it isn't, go back to square 1
        if (valid)
        {
            nextIndex++;
            if (nextIndex >= _emptyCellIndices.Length)
            {
                return true;
            }
        }
        else if (newValue >= 9)
        {
            // If we're stepping backwards, reset the current cell's value

            nextIndex = 0;
            // Keep backtracking until the puzzle is consistent
            for (int i = currentIndex; i >= 0; i--)
            {
                if (_puzzle.IsConsistent())
                {
                    nextIndex = i;
                    // If the last consistent cell was a 9, reset it and step back one more cell
                    if (_puzzle.GetValue(_emptyCellIndices[i]) == 9)
                    {
                        _puzzle.SetValue(_emptyCellIndices[i], 0);
                        nextIndex--;
                    }
                    break;
                }
               // Console.WriteLine($"Solver.Backtracker({currentIndex}). Resetting cell value for cell {_puzzle.GetCell(_emptyCellIndices[i])}.");
                _puzzle.SetValue(_emptyCellIndices[i], 0);
            }

            // Check if it's the last cell
            if (nextIndex == _emptyCellIndices.Length)
            {
               // Console.WriteLine($"Solver.Backtracker({currentIndex}). Next Index is outside the list: {nextIndex}.");
                return true;
            }
            if (nextIndex == currentIndex)
            {
               // Console.WriteLine($"Solver.Backtracker({currentIndex}). Previous Index is outside the list: {nextIndex}.");
                return false;
            }
        }

        // Console.WriteLine($"Solver.Backtracker({currentIndex}). Calling self on {nextIndex}.");
        // Console.WriteLine(_puzzle);
        // Console.WriteLine("[Press Enter to Continue]");
        // Console.ReadLine();
        return Backtracker(nextIndex);
    }

}