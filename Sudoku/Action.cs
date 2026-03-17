using System;
using System.Collections.Generic;

namespace Sudoku;


internal class Action : IEquatable<Action>
{
    internal ActionType Type {get; init;}

    internal int CellIndex {get; init;}
    internal int CellValue {get; init;}

    internal Action(ActionType resultType, int cellIndex, int cellValue)
    {
        Type = resultType;
        CellIndex = cellIndex;
        CellValue = cellValue;
    }

    public override string ToString()
    {
        string output = "";
        string? baseString = base.ToString();
        if (baseString != null)
        {
            output = baseString;
        }
        output += $"    Type={Type};";
        output += $"    CellIndex={CellIndex};";
        output += $"    CellValue={CellValue};";
        return output;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(Type, CellIndex, CellValue);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {   
            return false;
        }
        Action? objAsAction = obj as Action;
        if (objAsAction is null)
        {
            return false;
        }
        else
        {
            return Equals(objAsAction);
        }
    }

    public bool Equals(Action? other)
    {
        if (other is null)
        {
            return false;
        }
        
        return Type == other.Type &&
                CellIndex == other.CellIndex && 
                CellValue == other.CellValue;
    }

    public static bool operator ==(Action left, Action right)
    {
        if (left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(Action left, Action right)
    {
        return !(left == right);
    }
}