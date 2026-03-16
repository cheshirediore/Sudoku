using System;
using System.Collections.Generic;
using Sudoku.Utility;
using NUnit.Framework;


namespace Sudoku.Tests;

class ImporterTests
{
    [Test]
    public void Importer_PuzzleFromCSV_MissingFile_ThrowsArgumentException()
    {
        string filePath = System.IO.Path.GetFullPath("resources/FileNotFound.csv");

        Assert.That(() => Importer.PuzzleFromCSV(filePath),Throws.ArgumentException);
    }

    [Test]
    public void Importer_PuzzleFromCSV_FileNotCSV_ThrowsArgumentException()
    {
        string filePath = System.IO.Path.GetFullPath("resources/InvalidFileTypeTest.txt");
        Assert.That(() => Importer.PuzzleFromCSV(filePath),Throws.ArgumentException);
    }

    [Test]
    public void Importer_PuzzleFromCSV_ValidFile_ReturnsCorrectPuzzle()
    {
        /*
            8, 7, 5, 9, 2, 1, 3, 4, 6
            3, 6, 1, 7, 5, 4, 8, 9, 2
            2, 4, 9, 8, 6, 3, 7, 1, 5
            5, 8, 4, 6, 9, 7, 1, 2, 3
            7, 1, 3, 2, 4, 8, 6, 5, 9
            9, 2, 6, 1, 3, 5, 4, 8, 7
            6, 9, 7, 4, 1, 2, 5, 3, 8
            1, 5, 8, 3, 7, 9, 2, 6, 4
            4, 3, 2, 5, 8, 6, 9, 7, 1
        */

        string filePath = System.IO.Path.GetFullPath("resources/SamplePuzzle.csv");
        int[] expectedValues = [8, 7, 5, 9, 2, 1, 3, 4, 6, 3, 6, 1, 7, 5, 4, 8, 9, 2, 2, 4, 9, 8, 6, 3, 7, 1, 5, 5, 8, 4, 6, 9, 7, 1, 2, 3, 7, 1, 3, 2, 4, 8, 6, 5, 9, 9, 2, 6, 1, 3, 5, 4, 8, 7, 6, 9, 7, 4, 1, 2, 5, 3, 8, 1, 5, 8, 3, 7, 9, 2, 6, 4, 4, 3, 2, 5, 8, 6, 9, 7, 1];
        
        Puzzle puzzle = Importer.PuzzleFromCSV(filePath);

        for (int cellIndex = 0; cellIndex < expectedValues.Length; cellIndex++)
        {
            Assert.That(puzzle.GetCellValue(cellIndex), Is.EqualTo(expectedValues[cellIndex]));
        }

        // Update the file path to the empty seed
        filePath = System.IO.Path.GetFullPath("resources/EmptyPuzzleSeed.csv");
        Puzzle emptyPuzzle = Importer.PuzzleFromCSV(filePath);

        for (int cellIndex = 0; cellIndex < Grid.SIZE; cellIndex++)
        {
            Assert.That(emptyPuzzle.GetCellValue(cellIndex), Is.EqualTo(0));
        }
    }
}