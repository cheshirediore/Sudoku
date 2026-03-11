using System;
using System.Collections.Generic;

namespace Sudoku;

public class CellObserver : IObserver<Cell>
{
    private readonly Region _region;

    public CellObserver(Region region)
    {
        _region = region;
    }

    public void OnNext(Cell updatedCell)
    {
        foreach (var cell in _region.Cells)
        {
            if (cell != updatedCell)
            {
                // Update other cells in the region based on the updated cell
                cell.UpdateBasedOn(updatedCell);
            }
        }
    }

    public void OnError(Exception error)
    {
        // Handle errors
        Console.WriteLine($"Error observed: {error.Message}");
    }

    public void OnCompleted()
    {
        // Handle completion of observation
        Console.WriteLine("Observation completed.");
    }
}