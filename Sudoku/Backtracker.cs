using System;
using System.Collections.Generic;

namespace Sudoku;

public class Backtracker(Puzzle sudokuGrid) : Solver
{
    /// <summary>
    /// The puzzle the Backtracker will solve. It is updated with the solution.
    /// </summary>
    /// <remarks>
    /// Overrides the property for the Solver abstract class.
    /// </remarks>
    public override Puzzle SudokuPuzzle { get; init; } = sudokuGrid;
    /// <summary>
    /// The maximum number of solutions the Backtracker will search for. Once this number is reached, it will return the results without looking further.
    /// </summary>
    /// <remarks>
    /// Overrides the property for the Solver abstract class.
    /// </remarks>
    public override int MaxSolutions { get; set; } = -1;

    /// <summary>
    /// Invokes the top-level call of Backtrack to begin the recursive algorithm.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// Implements the method for the Solver abstract class.
    /// </remarks>
    public override List<Puzzle> Solve()
    {
        return Backtrack(SudokuPuzzle, []);
    }

    /// <summary>
    /// Implementation of the backtracking algorithm to search for valid configurations (i.e. solutions).
    /// </summary>
    /// <param name="candidate">
    /// A puzzle state.
    /// </param>
    /// <param name="solutions">
    /// The list of solutions so far.
    /// </param>
    /// <returns>
    /// The provided list of solutions, extended by any solutions found in this branch of the search tree.
    /// </returns>
    private List<Puzzle> Backtrack(Puzzle candidate, List<Puzzle> solutions)
    {
        // Stop looking if more than one solution has been found. We only care about valid sudoku puzzles.
        if (MaxSolutions > 0 && solutions.Count >= MaxSolutions)
        {
            return solutions;
        }
        if (Reject(candidate))
        {
            return solutions;
        }
        if (Accept(candidate))
        {
            return Output(candidate, solutions);
        }

        Puzzle? nextCandidate = First(candidate);
        while (nextCandidate != null)
        {
            solutions = Backtrack(nextCandidate, solutions);
            nextCandidate = Next(candidate, nextCandidate);
        }
        return solutions;
    }

    /// <summary>
    /// Adds a given solution to the running list of solutions.
    /// </summary>
    /// <param name="solution">
    /// The solution to be added to the list.
    /// </param>
    /// <param name="solutions">
    /// The list of solutions.
    /// </param>
    /// <returns></returns>
    /// <remarks>
    /// This method exists more for its conceptual role in the algorithm than out of necessity. 
    /// It continues to exist in case additional action should be desired when a solution is registered, such as an event trigger.
    /// </remarks>
    private static List<Puzzle> Output(Puzzle solution, List<Puzzle> solutions)
    {
        solutions.Add(solution);
        return solutions;
    }

    #region Validation
    /// <summary>
    /// Checks if a given <paramref name="candidate"/> is inconsistent. Used to prune dead ends in the search tree without having to walk all the way to the leaves.
    /// </summary>
    /// <param name="candidate">
    /// A puzzle state.
    /// </param>
    /// <returns>
    /// Returns true if and only if the <paramref name="candidate"/> is an invalid state (i.e. not worth completing).
    /// </returns>
    private static bool Reject(Puzzle candidate)
    {
        // Only check the last updated cell's row, column, and block.
        return !candidate.IsLastUpdateValid();
    }

    /// <summary>
    /// Validates that a given <paramref name="candidate"/> is complete and consistent. Used to check if a leaf in the search tree is a valid solution.
    /// </summary>
    /// <param name="candidate">
    /// A puzzle state.
    /// </param>
    /// <returns>
    /// Returns true if and only if <paramref name="candidate"/> is a solution to <see cref="Puzzle"/>
    /// </returns>
    private static bool Accept(Puzzle candidate)
    {
        if (Reject(candidate))
        {
            return false;
        }
        // Check that all cells are set
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (candidate.CellGrid.GetVertex(x, y).Value == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }
    #endregion

    #region Extenders
    /// <summary>
    /// Finds the first unset cell in the <paramref name="candidate"/> and sets it to the lowest valid value.
    /// </summary>
    /// <param name="candidate">
    /// A puzzle state.
    /// </param>
    /// <returns>
    /// Returns the first extension of the <paramref name="candidate"/> if a valid extension exists. Otherwise, returns null.
    /// </returns>
    private static Puzzle? First(Puzzle candidate)
    {
        // Make a shallow copy of the candidate
        Puzzle puzzle = (Puzzle)candidate.Clone();

        // Update the copy
        for (int y = 0; y < Grid<Cell>.HEIGHT; y++)
        {
            for (int x = 0; x < Grid<Cell>.WIDTH; x++)
            {
                if (candidate.CellGrid.GetVertex(x, y).Value == 0) // Find the first cell with an unset value in the partial candidate
                {
                    // If the value is less than 9, increment it. Otherwise, continue to the next cell.
                    if (puzzle.CellGrid.GetVertex(x, y).Value < 9)
                    {
                        puzzle.CellGrid.GetVertex(x, y).Value = candidate.CellGrid.GetVertex(x, y).Value + 1;
                        return puzzle;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Finds the next unset cell in the <paramref name="data"/> and increments the corresponding cell in <paramref name="candidate"/>.
    /// </summary>
    /// <param name="data">
    /// The puzzle state from which <paramref name="candidate"/> was extended.
    /// </param>
    /// <param name="candidate">
    /// The first extension of <paramref name="data"/>
    /// </param>
    /// <returns>
    /// Returns the next extension of the <paramref name="candidate"/> if a valid extension exists. Otherwise, returns null.
    /// </returns>
    private static Puzzle? Next(Puzzle data, Puzzle candidate)
    {
        // Make a shallow copy of the candidate
        Puzzle puzzle = (Puzzle)candidate.Clone();

        // Update the copy
        for (int y = 0; y < Grid<Cell>.HEIGHT; y++)
        {
            for (int x = 0; x < Grid<Cell>.WIDTH; x++)
            {
                if (data.CellGrid.GetVertex(x, y).Value == 0) // Find first cell with an unset value in the parent of the partial candidate
                {
                    // If the value is less than 9, increment it. Otherwise, continue to the next cell.
                    if (puzzle.CellGrid.GetVertex(x, y).Value < 9)
                    {
                        puzzle.CellGrid.GetVertex(x, y).Value = candidate.CellGrid.GetVertex(x, y).Value + 1;
                        return puzzle;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        return null;
    }
    #endregion
}