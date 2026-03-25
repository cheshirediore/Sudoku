using System;
using System.Collections.Generic;

namespace Sudoku.Utility;

public static class Importer
{
    /// <summary>
    /// Constructs a Puzzle object using values provided by a csv file.
    /// </summary>
    /// <param name="filePath">
    /// File path should indicate a csv file with 9 lines, and 9 columns. Each column must contain an integer
    /// value between 0 and 9 (inclusive), where 0 indicates an unset value.
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when the input file is not the appropriate width, height, or when it contains non-numeric values.
    /// </exception>
    /// <exception cref="System.ArgumentException">
    /// Thrown when the file path provided does not exist or is not a CSV.
    /// </exception>
    public static Puzzle PuzzleFromCSV(string filePath)
    {
        // Verify that the file exists
        if (!System.IO.File.Exists(filePath))
        {
            throw new ArgumentException($"The file at '{filePath}' does not exist.");
        }

        // Verify that the file is a CSV file
        if (!filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"The file at '{filePath}' is not a CSV file.");
        }

        // Initialize a list to hold the cell values from the file. 
        // Using a list instead of an array because it saves calculating the index from (x,y), and
        // the importer isn't run often enough for the additional overhead to matter.
        List<int> cellValues = [];

        // Open the file, read the content, and close it
        string fileContent = System.IO.File.ReadAllText(filePath);

        // Split the content by lines, and verify that the line count is correct
        string[] lines = fileContent.Split("\n");
        if (lines.Length != Grid.HEIGHT)
        {
            throw new ArgumentOutOfRangeException(filePath, $"Input puzzle seed must have {Grid.HEIGHT} lines. Provided seed has '{lines.Length}'.");
        }

        // Iterate over the lines and add the values to the vertex grid
        for (int y = 0; y < Grid.HEIGHT; y++)
        {
            // Split the line by commas, and trim off the whitespace
            string[] rowValues = lines[y].Split(",");

            // Verify that the width is correct before adding it to the cellValues
            if (rowValues.Length != Grid.WIDTH)
            {
                throw new ArgumentOutOfRangeException(filePath, $"'{rowValues.Length}' is an invalid width. All rows in the input puzzle seed must have a width of {Grid.WIDTH}.");
            }

            // Verify that each string is numeric, and add it to the cellValues iff it is. Otherwise, throw an exception.
            for (int x = 0; x < rowValues.Length; x++)
            {
                if (int.TryParse(rowValues[x].Trim(), out int parsedValue))
                {
                    cellValues.Add(parsedValue);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(filePath, $"Invalid value passed in puzzle seed. Check file for non-numeric characters.");
                }
            }
        }

        // Once we've determined that input file is valid, we can make a new Puzzle and start assigning values
        Puzzle puzzle = new();
        for (int cellIndex = 0; cellIndex < Grid.SIZE; cellIndex++)
        {
            bool valuesAsClues = cellValues[cellIndex] > 0;
            puzzle.SetCellValue(cellIndex, cellValues[cellIndex], valuesAsClues);
        }

        return puzzle;
    }
}