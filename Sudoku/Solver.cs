namespace Sudoku;

/// <summary>
/// Given an incomplete <c>Puzzle</c>, the <c>Solver</c> determines a valid solution (if possible).
/// </summary>
public class Solver
{
    private Puzzle _puzzle;

    public Solver(Puzzle puzzle)
    {
        _puzzle = puzzle;
    }

    public void Solve()
    {
        Backtracker();
    }
    
    private bool Backtracker()
    {
        // Set the current cell as the first cell
        // Process the first cell

        // Iterate through each cell of the puzzle

        // Process each cell, and check the return value
            // If false, check if the current cell is the first cell
                // If so, return false. This indicates there is no valid solution to the puzzle
            // Otherwise, set the current cell to the previous cell
            // If true, set the current cell to the next cell
        
        // If it reaches the end, it has found a valid solution
        return true;
    }
    
    // Increments the given cell's value until it either satisfies the sudoku condition, or reaches 9
    // and still violates it.
    // Returns true when it finds a working value.
    // Returns false if it fails to find one.
    private bool ProcessCell()
    {
        // Check if cell is a clue. If so, return true immediately.
        // Check cell value. If it's already 9 (and not a clue), return false immediately.

        // Increment cell's value
        // Check if the sudoku puzzle is still valid
        // If so, return true

        // Otherwise, check if the cell value is 9
        // If so, return false
        // Otherwise, continue iterating and incrementing values
        return false;
    }
}