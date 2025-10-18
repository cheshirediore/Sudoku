namespace Sudoku
{
    public class Cell : IEquatable<Cell>
    {
        // "attribute"
        int[] _coordinates;
        // "property"
        public int X {
            get => this._coordinates[0];
            set => this._coordinates[0] = value;
        }
        // "property"
        public int Y {
            get => this._coordinates[1];
            set => this._coordinates[1] = value;
        }

        // "attribute"
        int _value;
        // "property"
        public int Value {
            get => this._value;
            set => this._value = value;
        }

        #region Constructors
        public Cell(int x, int y, int cellValue) 
        {
            this._coordinates = new int[2] {x, y};
            this._value = cellValue;
        }

        public Cell(int x, int y) : this(x, y, 0)
        {
        }

        public Cell() : this(0, 0)
        {
        }
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