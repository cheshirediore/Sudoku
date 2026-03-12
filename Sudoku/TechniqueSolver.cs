using System.Collections.Generic;

namespace Sudoku;

public class TechniqueSolver(Puzzle sudokuGrid) : Solver
{
    #region SolverInterface
    /// <summary>
    /// The puzzle to be solved. It is updated with the solution.
    /// </summary>
    /// <remarks>
    /// Overrides the property for the Solver abstract class.
    /// </remarks>
    public override Puzzle SudokuPuzzle { get; init; } = sudokuGrid;
    /// <summary>
    /// The maximum number of solutions to search for. Once this number is reached, 
    /// it will return the results without looking further.
    /// </summary>
    /// <remarks>
    /// Overrides the property for the Solver abstract class.
    /// </remarks>
    public override int MaxSolutions { get; set; } = -1;

    public override List<Puzzle> Solve()
    {
        // TODO: loop through applying rules to the current board state to determine
        //       the next valid move. Terminate and hand off to the Backtracker if 
        //       no valid moves are determined by applying rules.
        throw new System.NotImplementedException();
    }

    #endregion


    private delegate List<int[]> Rule();

    /// <summary>
    /// Get the next rule to apply, based on the previous rule.
    /// </summary>
    private Rule GetNextRule(Rule? lastRule)
    {
        // First, look for Naked Single candidates
        // Next, look for Hidden Single candidates
        // Next, look for Naken Pair candidates
        // Next, look for Hidden Pair candidates
        // ...
        // TODO: Add the rest of the rule orders
        return NakedSingle;
    }
    
    #region Rules
    /// <summary>
    /// Identify the Naked Single candidates, if they exists.
    /// </summary>
    /// <returns>
    /// A list of integer arrays of the format [value, x, y], where value is what the
    /// cell should be set to, and x, y are the coordinates of the cell.
    /// 
    /// If no such candidates exist, returns an empty list.
    /// </returns>
    private List<int[]> NakedSingle()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        List<int[]> results = [];
        // For each region, identify any Naked Single candidates.
        // Search the Blocks
        // Search the Columns
        // Search the Rows
        return results;
    }

    /// <summary>
    /// Identify the first Naked Single candidates, if they exists.
    /// </summary>
    /// <returns>
    /// A list of integer arrays of the format [value, x, y], where value is what the
    /// cell should be set to, and x, y are the coordinates of the cell.
    /// 
    /// If no such candidates exist, returns an empty list.
    /// </returns>
    private List<int[]> HiddenSingle()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        List<int[]> results = [];
        return results;
    }
    
    /// <summary>
    /// Identify the first Naked Pair candidates, if they exists.
    /// </summary>
    /// <returns>
    /// A list of integer arrays of the format [value, x, y], where value is what the
    /// cell should be set to, and x, y are the coordinates of the cell.
    /// 
    /// If no such candidates exist, returns an empty list.
    /// </returns>
    private List<int[]> NakedPair()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        List<int[]> results = [];
        return results;
    }

    /// <summary>
    /// Identify the Hidden Pair candidates, if they exists.
    /// </summary>
    /// <returns>
    /// A list of integer arrays of the format [value, x, y], where value is what the
    /// cell should be set to, and x, y are the coordinates of the cell.
    /// 
    /// If no such candidates exist, returns an empty list.
    /// </returns>
    private List<int[]> HiddenPair()
    {
        throw new System.NotImplementedException();
        // Initialize the empty list for the return
        List<int[]> results = [];
        return results;
    }
    #endregion
}