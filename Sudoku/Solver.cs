using System;
namespace Sudoku;

/// <summary>
/// Given an incomplete <c>Puzzle</c>, the <c>Solver</c> determines a valid solution (if possible).
/// </summary>
public class Solver
{
    private Puzzle _puzzle;
    // This list contains the indices of the empty cells in the puzzle.
    private int[] _emptyCellIndices;

    public Solver(Puzzle puzzle)
    {
        _puzzle = puzzle;
        _emptyCellIndices = _puzzle.GetEmptyCellIndices();
    }

    public int Solve()
    {
        int solutionsFound = 0;
        // Set the current cell as the first cell
        bool success = Backtracker(0);
        // If successful, incrememnt count and look for more solutions
        if (success)
        {
            Console.WriteLine($"Solution Found:\n{_puzzle}");
            solutionsFound++;

            if (_emptyCellIndices.Length > 1)
            {
                // Clear cells from the previous solution
                for (int i = 1; i < _emptyCellIndices.Length; i++)
                {
                    _puzzle.ClearValue(_emptyCellIndices[i]);
                }
            }

            success = Backtracker(0);
            if (success)
            {
                Console.WriteLine($"Solution Found:\n{_puzzle}");
                solutionsFound++;
            }
        }

        

        return solutionsFound;
    }

    private bool Backtracker(int currentIndex)
    {
        // The currentIndex and nextIndex variables refer to the _emptyCellIndices, now the puzzle's indices.

        #region BaseConditions
        // Base Conditions:
        //     1) If index is off the beginning of the list, return failure
        //     2) If the puzzle has no empty cells, return consistency
        //     3) If index is off the end of the list, return puzzle consistency

        // Condition 1: Failure
        if (currentIndex < 0)
        {
            Console.WriteLine($"Current Index = {currentIndex}. Returning false.");
            return false;
        }

        // Condition 2: Possible Success
        if (_emptyCellIndices.Length == 0)
        {
            Console.WriteLine("No empty cells. Returning consistency.");
            return _puzzle.IsConsistent();
        }

        if (currentIndex >= _emptyCellIndices.Length)
        {
            Console.WriteLine("Index larger than the list size. Returning consistency.");
            return _puzzle.IsConsistent();
        }
        #endregion
        
        // Try to increment the current cell's value
        int newValue = _puzzle.GetValue(_emptyCellIndices[currentIndex]) + 1;
        if (newValue <= 9)
        {
            // Increment the cell
            _puzzle.SetValue(_emptyCellIndices[currentIndex], newValue);
        }

        #region DirectionDeterminate
        int nextIndex = currentIndex;
        // Check if the puzzle is consistent
        bool consistent = _puzzle.IsConsistent();

        // If it's consistent, set pointer forward. If it isn't, set pointer backward.
        if (consistent)
        {
            nextIndex++;
        }
        else if (newValue >= 9)
        {
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
                        // Decrement the index an additional time when a 9 is found
                        nextIndex--;
                    }
                    break;
                }

                // If we're stepping backwards, reset the current cell's value
                _puzzle.SetValue(_emptyCellIndices[i], 0);
            }
        }
        #endregion
        // Recurse
        return Backtracker(nextIndex);
    }


}