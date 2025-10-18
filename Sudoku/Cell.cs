namespace Sudoku;

/// <summary>
/// Class <c>Cell</c> models the individual squares in a sudoku puzzle.
/// Each cell has a value between 1 and 9 (inclusive) that can be either hidden or visible.
/// 
/// The Puzzle object acts as a gatekeeper to instances of this class to maintain integrity of the Puzzle's grid.
/// </summary>
public class Cell(int x, int y, int cellValue) : IEquatable<Cell>
{
    private int[] _coordinates = new int[2] { x, y };
    private int _value = cellValue;
    private bool _visible = false;


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

    public bool Visible
    {
        get => _visible;
        set => _visible = value;
    }

    #region Constructors
    public Cell(int x, int y) : this(x, y, 0) { }

    public Cell() : this(0, 0) { }
    #endregion

    public bool IsSet()
    {
        return Value > 0 && Value < 10;
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
}