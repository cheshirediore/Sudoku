namespace Sudoku;

/// <summary>
/// Given an incomplete <c>Puzzle</c>, the <c>Solver</c> determines a valid solution (if possible).
/// </summary>
public class Solver
{
    public static void Backtracker(Puzzle puzzle)
    {
        // Briefly, a program would solve a puzzle by placing the digit "1" in the first cell 
        // and checking if it is allowed to be there. If there are no violations (checking row, 
        // column, and box constraints) then the algorithm advances to the next cell and places 
        // a "1" in that cell. When checking for violations, if it is discovered that the "1" 
        // is not allowed, the value is advanced to "2". If a cell is discovered where none of 
        // the 9 digits is allowed, then the algorithm leaves that cell blank and moves back 
        // to the previous cell. The value in that cell is then incremented by one. This is 
        // repeated until the allowed value in the last (81st) cell is discovered.

        // For each cell in the puzzle grid
        //      Check if the cell is set
        //          If not, place a 1
        //          Check if the cell is valid
        //          If not, clear the cell and go back

        
    }
}