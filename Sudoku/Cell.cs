using System;
using System.Collections.Generic;

namespace Sudoku;

public class Cell : ICloneable, IEquatable<Cell>
{
    public int Index {get; init;}
    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            if (_value != 0) Candidates.Clear(); // Clear the candidates list iff we set it to a legit value
            Notifier.Notify(this);
        }
    }
    private int _value;
    public List<int> Candidates;
    public bool IsClue;

    public CellNotifier Notifier;

    public Cell(): this(-1)
    {}

    public Cell(int index)
    {
        Index = index;
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
    internal bool RemoveCandidate(int candidate)
    {
        // if (Index == 79) Console.WriteLine($"Removing candidate '{candidate}' from cell #{Index}");
        bool removedCount = Candidates.Remove(candidate);
        //Candidates.Sort();
        return removedCount;
    }

    /// <summary>
    /// Adds a given candidate to the list of candidates.
    /// </summary>
    /// <param name="candidate"></param>
    /// <returns>True if the given candidate was removed. False otherwise.</returns>
    internal void AddCandidate(int candidate)
    {
        Candidates.Add(candidate);
        Candidates.Sort();
    }

    /// <summary>
    /// Removes the updated cell's value from this cell's list of candidates.
    /// </summary>
    /// <param name="neighborCell"></param>
    /// <returns>True if the given candidate was removed. False otherwise.</returns>
    public bool UpdateBasedOn(Cell neighborCell)
    {
        // if (Index == 79 || neighborCell.Index == 79) Console.WriteLine($"[Cell.UpdateBasedOn] Updating ({this}) based on ({neighborCell})");
        if (neighborCell.Value == 0) return false; // don't bother trying to remove invalid value

        return RemoveCandidate(neighborCell.Value);
    }

    public object Clone()
    {
        Cell clonedCell = new(this.Index)
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
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        return Index + Value + Candidates.GetHashCode() + Notifier.GetHashCode();
    }

    public bool Equals(Cell? other)
    {
        if (other == null)
        {
            return false;
        }
        
        return GetHashCode() == other.GetHashCode();
    }

    public override string ToString()
    {
        string output = "";
        string? baseString = base.ToString();
        if (baseString != null)
        {
            output = baseString;
        }
        string candidates = "";
        foreach (var item in Candidates)
        {
            candidates += $"{item}, ";
        }

        output += $"   Index={Index}";
        output += $"   Value={Value}";
        output += $"   IsClue={IsClue}";
        output += $"   Candidates={candidates}";

        return output;
    }
}