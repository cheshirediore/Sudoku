using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Sudoku.Tests;

[TestFixture]
public class TechniqueSolverTests
{

    // [Test]
    // public void TechniqueSolver_Solve_ValidPuzzle_ReturnIsNotEmpty()
    // {
    //     // Given: The first row of the puzzle has values 1-8 set, with 9 as the only candidate for the last cell.
    //     Puzzle puzzle = new();
    //     // TODO: set up a valid puzzle seed

    //     // Given: A new TechniqueSolver is created for the puzzle
    //     TechniqueSolver solver = new(puzzle);

    //     // When: Hidden Singles are checked and applied
    //     List<Puzzle> solutions = solver.Solve();

    //     // Then: Cell index 8 is set to 9
    //     Assert.That(solutions, Is.Not.Empty);
    // }

    // public void TechniqueSolver_Solve_InValidPuzzle_ReturnIsEmpty()
    // {
    //     // Given: The first row of the puzzle has values 1-8 set, with 9 as the only candidate for the last cell.
    //     Puzzle puzzle = new();
    //     puzzle.SetCellValue(0, 1, true);
    //     puzzle.SetCellValue(1, 1, true);

    //     // Given: A new TechniqueSolver is created for the puzzle
    //     TechniqueSolver solver = new(puzzle);

    //     // When: Hidden Singles are checked and applied
    //     List<Puzzle> solutions = solver.Solve();

    //     // Then: Cell index 8 is set to 9
    //     Assert.That(solutions, Is.Empty);
    // }

    [Test]
    public void TechniqueSolver_Solve_NakedSingle_Clues_ShouldUpdateValues()
    {
        // Given: The first row of the puzzle has values 1-8 set as clues, with 9 as the only candidate for the last cell.
        Puzzle puzzle = new();
        puzzle.SetCellValue(0, 1, true);
        puzzle.SetCellValue(1, 2, true);
        puzzle.SetCellValue(2, 3, true);
        puzzle.SetCellValue(3, 4, true);
        puzzle.SetCellValue(4, 5, true);
        puzzle.SetCellValue(5, 6, true);
        puzzle.SetCellValue(6, 7, true);
        puzzle.SetCellValue(7, 8, true);
        // Given: A new TechniqueSolver is created for the puzzle
        TechniqueSolver solver = new(puzzle);

        // When: Naked Singles are checked and applied
        List<Puzzle> solutions = solver.Solve();

        // Then: Cell index 8 is set to 9
        Assert.That(9, Is.EqualTo(solutions[0].GetCellValue(8)));
    }

    [Test]
    public void TechniqueSolver_Solve_NakedSingle_NonClues_ShouldUpdateValues()
    {
        // Given: The first row of the puzzle has values 1-8 set (NOT clues), with 9 as the only candidate for the last cell.
        Puzzle puzzle = new();
        puzzle.SetCellValue(0, 1, false);
        puzzle.SetCellValue(1, 2, false);
        puzzle.SetCellValue(2, 3, false);
        puzzle.SetCellValue(3, 4, false);
        puzzle.SetCellValue(4, 5, false);
        puzzle.SetCellValue(5, 6, false);
        puzzle.SetCellValue(6, 7, false);
        puzzle.SetCellValue(7, 8, false);
        // Given: A new TechniqueSolver is created for the puzzle
        TechniqueSolver solver = new(puzzle);

        // When: Naked Singles are checked and applied
        List<Puzzle> solutions = solver.Solve();

        // Then: Cell index 8 is set to 9
        Assert.That(9, Is.EqualTo(solutions[0].GetCellValue(8)));

    }

    [Test]
    public void TechniqueSolver_NakedSingle_ShouldReturnValues()
    {
        // Given: The first row of the puzzle has values 1-8 set, with 9 as the only candidate for the last cell.
        Puzzle puzzle = new();
        puzzle.SetCellValue(0, 1);
        puzzle.SetCellValue(1, 2);
        puzzle.SetCellValue(2, 3);
        puzzle.SetCellValue(3, 4);
        puzzle.SetCellValue(4, 5);
        puzzle.SetCellValue(5, 6);
        puzzle.SetCellValue(6, 7);
        puzzle.SetCellValue(7, 8);
        // Given: A new TechniqueSolver is created for the puzzle
        TechniqueSolver solver = new(puzzle);

        // When: Hidden Singles are checked
        List<int[]> hiddenSingles = solver.NakedSingle();

        // Then: Hidden Singles are found
        Assert.That(hiddenSingles, Is.Not.Empty);
    }

    [Test]
    public void TechniqueSolver_HiddenSingle_ShouldReturnValues()
    {
        /* Index structure reference

              0  1  2    3  4  5    6  7  8

        0     0  1  2 |  3  4  5 |  6  7  8
        1     9 10 11 | 12 13 14 | 15 16 17
        2    18 19 20 | 21 22 23 | 24 25 26
             ------------------------------
        3    27 28 29 | 30 31 32 | 33 34 35
        4    36 37 38 | 39 40 41 | 42 43 44
        5    45 46 47 | 48 49 50 | 51 52 53
             ------------------------------
        6    54 55 56 | 57 58 59 | 60 61 62
        7    63 64 65 | 66 67 68 | 69 70 71
        8    72 73 74 | 75 76 77 | 78 79 80
        */
        // Given: The second row, third row, second column, and third column contain a 1
        Puzzle puzzle = new();
        puzzle.SetCellValue(28, 1); // Second column, fourth block
        puzzle.SetCellValue(56, 1); // Third column, seventh block
        puzzle.SetCellValue(13, 1); // Second row, second block
        puzzle.SetCellValue(25, 1); // Third row, third block
        
        // Given: A new TechniqueSolver is created for the puzzle
        TechniqueSolver solver = new(puzzle);

        // When: Hidden Singles are checked
        List<int[]> hiddenSingles = solver.HiddenSingle();

        // Then: Hidden Singles are found
        Assert.That(hiddenSingles, Is.Not.Empty);
    }

    [Test]
    public void TechniqueSolver_HiddenSingle_ShouldReturnCorrectValues()
    {
        /* Index structure reference

              0  1  2    3  4  5    6  7  8

        0     0  1  2 |  3  4  5 |  6  7  8
        1     9 10 11 | 12 13 14 | 15 16 17
        2    18 19 20 | 21 22 23 | 24 25 26
             ------------------------------
        3    27 28 29 | 30 31 32 | 33 34 35
        4    36 37 38 | 39 40 41 | 42 43 44
        5    45 46 47 | 48 49 50 | 51 52 53
             ------------------------------
        6    54 55 56 | 57 58 59 | 60 61 62
        7    63 64 65 | 66 67 68 | 69 70 71
        8    72 73 74 | 75 76 77 | 78 79 80
        */
        // Given: The second row, third row, second column, and third column contain a 1
        Puzzle puzzle = new();
        puzzle.SetCellValue(28, 1); // Second column, fourth block
        puzzle.SetCellValue(56, 1); // Third column, seventh block
        puzzle.SetCellValue(13, 1); // Second row, second block
        puzzle.SetCellValue(25, 1); // Third row, third block
        
        // Given: A new TechniqueSolver is created for the puzzle
        TechniqueSolver solver = new(puzzle);

        // When: Hidden Singles are checked
        List<int[]> hiddenSingles = solver.HiddenSingle();

        // Then:
        if (hiddenSingles.Count > 0)
        {
            Assert.That(hiddenSingles[0][0], Is.EqualTo(0)); // Index of hidden single is 0
            Assert.That(hiddenSingles[0][1], Is.EqualTo(1)); // Value of hidden single is 1
        } else
        {
            Assert.That(false, Is.True, "HiddenSingle returned an empty list.");
        }
        
    }
}