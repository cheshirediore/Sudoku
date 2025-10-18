namespace Sudoku;

/// <summary>
/// Class <c>Puzzle</c> models the sudoku puzzle itself.
/// </summary>
public class Puzzle
{

    private int[] _dimensions = new int[2];
    private Cell[,] _grid;

    public int Width
    {
        get => _dimensions[0];
        set => _dimensions[0] = value;
    }
    public int Height
    {
        get => _dimensions[1];
        set => _dimensions[1] = value;
    }


    public Puzzle(int width, int height)
    {
        Width = width;
        Height = height;
        _grid = new Cell[Width, Height];
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _grid[x, y] = new Cell();
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        return _grid[x, y];
    }
}