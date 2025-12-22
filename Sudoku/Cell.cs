using System;
namespace Sudoku;
// DEPRECATED

/// <summary>
/// Class <c>Cell</c> models the individual squares in a sudoku puzzle.
/// Each cell has a value between 1 and 9 (inclusive) that is hidden from the player.
/// It also has a value between 1 and 9 (inclusive) that is revealed to the player, and
/// is updated by the player as they solve the puzzle.
/// 
/// The Puzzle object acts as a gatekeeper to instances of this class to maintain integrity of the Puzzle's grid.
/// </summary>
public class Cell(int x, int y, int cellValue) : IEquatable<Cell>
{
    private int[] _coordinates = new int[2] { x, y };
    private int _value = cellValue;
    private bool _isClue = false;

    public bool IsClue
    {
        get => _isClue;
        set => _isClue = value;
    }


    // Accessors for the X and Y coordinates
    public int X
    {
        get => _coordinates[0];
        set => _coordinates[0] = value;
    }

    public int Y
    {
        get => _coordinates[1];
        set => _coordinates[1] = value;
    }

    // Accessor for the cell's value
    public int Value
    {
        get => _value;
        set => _value = value;
    }

    public int PlayerValue;

    #region Constructors
    public Cell(int x, int y) : this(x, y, 0) { }

    public Cell() : this(0, 0) { }
    #endregion

    public bool IsSet()
    {
        return Value > 0 && Value < 10;
    }

    // Clues are the pre-filled cells in the puzzle that display their value to the player
    public void SetClue(int clueValue)
    {
        _value = clueValue;
        _isClue = true;
    }

    #region InterfaceImplementation
    public bool Equals(Cell? other)
    {
        if (other is null)
        {
            return false;
        }
        return this.X == other.X && this.Y == other.Y;
    }
    #endregion

    public override string ToString()
    {
        return $"({X}, {Y}). Value={Value}";
    }
}