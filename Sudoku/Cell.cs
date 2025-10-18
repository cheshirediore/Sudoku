namespace Sudoku
{
    public class Cell(int x, int y, int cellValue) : IEquatable<Cell>
    {
        int[] _coordinates = new int[2] { x, y };
        int _value = cellValue;


        // Accessors for the X and Y coordinates
        public int X {
            get => this._coordinates[0];
            set => this._coordinates[0] = value;
        }

        public int Y {
            get => this._coordinates[1];
            set => this._coordinates[1] = value;
        }

        // Accessor for the cell's value
        public int Value
        {
            get => this._value;
            set => this._value = value;
        }

        #region Constructors
        public Cell(int x, int y) : this(x, y, 0) { }

        public Cell() : this(0, 0) { }
        #endregion

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
}