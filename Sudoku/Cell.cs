using System;
using System.Collections.Generic;

namespace Sudoku;

public class Cell : ICloneable
{
    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            Notifier.Notify(this);
        }
    }
    private int _value;
    public List<int> Candidates;
    public bool IsClue;

    public CellNotifier Notifier;

    public Cell()
    {
        _value = 0;
        Candidates = [1, 2, 3, 4, 5, 6, 7, 8, 9];
        IsClue = false;
        Notifier = new CellNotifier();
    }

    /// <summary>
    /// Removes a given candidate from the list of candidates.
    /// </summary>
    /// <param name="candidate"></param>
    /// <returns>True if the given candidate was removed. False otherwise.</returns>
    public bool RemoveCandidate(int candidate)
    {
        int removedCount = Candidates.RemoveAll(c => c == candidate);
        return removedCount > 0;
    }

    /// <summary>
    /// Removes the updated cell's value from this cell's list of candidates.
    /// </summary>
    /// <param name="neighborCell"></param>
    /// <returns>True if the given candidate was removed. False otherwise.</returns>
    public bool UpdateBasedOn(Cell neighborCell)
    {
        return RemoveCandidate(neighborCell.Value);
    }

    public object Clone()
    {
        Cell clonedCell = new()
        {
            IsClue = this.IsClue,
            _value = this._value
        };

        clonedCell.Candidates.Clear();
        foreach (var c in Candidates)
        {
            clonedCell.Candidates.Add(c);
        }
        return clonedCell;
    }
}