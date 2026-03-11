using System;
using System.Collections.Generic;

namespace Sudoku;

public enum RegionType
{
    BLOCK,
    COLUMN,
    ROW
}

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
}