using System;
using System.Collections.Generic;

namespace Sudoku;

public class Region
{
    private const int CAPACITY = 9;
    private readonly object _lock = new();

    public RegionType Type {get; init;}
    public IReadOnlyList<Cell> Cells
    {
        get
        {
            lock (_lock)
            {
                return _cells.AsReadOnly();
            }
        }
    } 
    private List<Cell> _cells;
    private CellObserver _cellObserver;

    public Region(RegionType regionType)
    {
        if (!Enum.IsDefined(regionType))
        {
            throw new ArgumentException("Invalid region type.", nameof(regionType));
        }

        Type = regionType;
        _cells = [];
        _cellObserver = new CellObserver(this);
    }

    public bool AddCell(Cell cell)
    {
        if (cell == null)
        {
            throw new ArgumentNullException(nameof(cell), "Cell cannot be null.");
        }

        lock (_lock)
        {
            if (_cells.Count >= CAPACITY)
            {
                throw new InvalidOperationException($"Region can only contain {CAPACITY} cells. Cannot add more to this region.");
            }
            
            _cells.Add(cell);
            cell.Notifier.Subscribe(_cellObserver); // Register cell with the observer
            return true;
        }
    }

    public bool IsConsistent()
    {
        HashSet<int> values = new();

        for (int index = 0; index < CAPACITY; index++)
        {
            if (Cells[index].Value != 0 && !values.Add(System.Math.Abs(Cells[index].Value)))
            {
                return false;
            }
        }
        return true;
    }
    
    // TODO: Implement a method to count candidates of cells in a region, to replace the repetitive code in
    //       TechniqueSolver
}