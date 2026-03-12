using System;
using System.Collections.Generic;

namespace Sudoku;
// TODO: Build out constructor(s)
// TODO: add regions to puzzle
// TODO: add grid to puzzle
// TODO: create all 81 cells and add them to their regionspublic class Puzzle
public class Puzzle : ICloneable
{
    private const int REGIONS_PER_TYPE = 9;

    public Grid<Cell> CellGrid; // Models the catesian plane
    public Dictionary<RegionType, List<Region>> Regions; // Divides the grid into 27 regions

    public Puzzle() : this(new Grid<Cell>())
    {
    }
        

    public Puzzle(Grid<Cell> cellGrid)
    {
        CellGrid = cellGrid;

        // Initialize empty regions
        Regions = [];
        Regions[RegionType.BLOCK] = [];
        Regions[RegionType.COLUMN] = [];
        Regions[RegionType.ROW] = [];

        for (int i = 0; i < REGIONS_PER_TYPE; i++)
        {
            Regions[RegionType.BLOCK].Add(new Region(RegionType.BLOCK));
            Regions[RegionType.COLUMN].Add(new Region(RegionType.COLUMN));
            Regions[RegionType.ROW].Add(new Region(RegionType.ROW));
        }

        // Add cell references to regions
        for (int cellIndex = 0; cellIndex < Grid<Cell>.SIZE; cellIndex++)
        {
            Cell cell = CellGrid.GetVertex(cellIndex);
            int blockIndex = GetRegionIndex(cellIndex, RegionType.BLOCK);
            int columnIndex = GetRegionIndex(cellIndex, RegionType.COLUMN);
            int rowIndex = GetRegionIndex(cellIndex, RegionType.ROW);
            GetRegion(RegionType.BLOCK, blockIndex).AddCell(cell);
            GetRegion(RegionType.COLUMN, columnIndex).AddCell(cell);
            GetRegion(RegionType.ROW, rowIndex).AddCell(cell);
        }
    }

    #region Region Methods
    /// <summary>
    /// Provides access to the 9 blocks, 9 columns, and 9 rows of the grid
    /// </summary>
    /// <param name="regionType"></param>
    /// <param name="regionIndex"></param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException"></exception>
    public Region GetRegion(RegionType regionType, int regionIndex)
    {
        // Validate RegionType
        if (!Enum.IsDefined(regionType))
        {
            throw new ArgumentOutOfRangeException(nameof(regionType), $"Invalid RegionType value {regionType}.");
        }
        // Validate region index
        if (regionIndex < 0 || regionIndex >= REGIONS_PER_TYPE)
        {
            throw new ArgumentOutOfRangeException(nameof(regionIndex), $"Invalid regionIndex value {regionIndex}.");
        }

        return Regions[regionType][regionIndex];        
    }

    public static int GetRegionIndex(int cellIndex, RegionType regionType)
    {
        // Validate RegionType
        if (!Enum.IsDefined(regionType))
        {
            throw new ArgumentOutOfRangeException(nameof(regionType), $"Invalid RegionType value {regionType}.");
        }
        // Validate region index
        if (cellIndex < 0 || cellIndex >= Grid<Cell>.SIZE)
        {
            throw new ArgumentOutOfRangeException(nameof(cellIndex), $"Invalid regionIndex value {cellIndex}.");
        }

        // The region is easier to determine from the coordinates than the index
        int[] coordinates = Grid<Cell>.IndexToCoordinates(cellIndex);
        int x = coordinates[0];
        int y = coordinates[1];

        // Blocks
        if (regionType == RegionType.BLOCK)
        {
            // Not a very elegant solution, but it works.

            // Top 3 Rows
            if (y < 3)
            {
                // Left 3 columns
                if (x < 3)
                {
                    return 0;
                }
            
                // Middle 3 columns
                if (x > 2 && x < 6)
                {
                    return 1;
                }

                // Right 3 columns
                if (x > 5)
                {
                    return 2;
                }
            }

            // Middle 3 Rows
            if (y > 2 && y < 6 )
            {
                // Left 3 columns
                if (x < 3)
                {
                    return 3;
                }
            
                // Middle 3 columns
                if (x > 2 && x < 6)
                {
                    return 4;
                }

                // Right 3 columns
                if (x > 5)
                {
                    return 5;
                }
            }

            // Bottom 3 Rows
            if (y > 5)
            {
                // Left 3 columns
                if (x < 3)
                {
                    return 6;
                }
            
                // Middle 3 columns
                if (x > 2 && x < 6)
                {
                    return 7;
                }

                // Right 3 columns
                if (x > 5)
                {
                    return 8;
                }
            }
            
        }
        // Columns
        if (regionType == RegionType.COLUMN)
        {
            return x;
        }
        // Rows
        if (regionType == RegionType.ROW)
        {
            return y;
        }

        // Failure case
        return -1;
    }
    #endregion

    public bool SetCellValue(int cellIndex, int newValue)
    {
        Cell cell = CellGrid.GetVertex(cellIndex);
        cell.Value = newValue;
        return true;
    }

    /// <summary>
    /// Checks if there are any duplicate values in any regions
    /// </summary>
    /// <returns></returns>
    public bool IsConsistent()
    {
        for (int regionIndex = 0; regionIndex < 9; regionIndex++)
        {
            // Check the block
            if (!GetRegion(RegionType.BLOCK, regionIndex).IsConsistent())
            {
                return false;
            }
            // Check the column
            if (!GetRegion(RegionType.COLUMN, regionIndex).IsConsistent())
            {
                return false;
            }
            // Check the row
            if (!GetRegion(RegionType.ROW, regionIndex).IsConsistent())
            {
                return false;
            }
        }
        // If all blocks, columns, and rows are internally consistent, then the puzzle is internally consistent
        return true;
    }

    /// <summary>
    /// Check if all cells have a value set
    /// </summary>
    /// <returns>False if any cell retains the default value, and true otherwise.</returns>
    public bool IsComplete()
    {
        for (int cellIndex = 0; cellIndex < Grid<Cell>.SIZE; cellIndex++)
        {
            if (CellGrid.GetVertex(cellIndex).Value == 0)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsLastUpdateValid()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        System.Text.StringBuilder builder = new();

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                int cellValue = CellGrid.GetVertex(x, y).Value;
                string prefix = cellValue < 0 ? "": " ";
                builder.Append($"{prefix}{cellValue} ");
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }

    /// <remarks>
    /// The regions are generated dynamically from the initial Grid<Cell>, so we don't
    /// need to clone those; they will get "cloned" automatically when the new puzzle
    /// is initialized with the cloned grid.
    /// </remarks>
    public object Clone()
    {
        Grid<Cell> cellGrid = (Grid<Cell>)this.CellGrid.Clone();

        Puzzle clonedPuzzle = new(cellGrid);

        return clonedPuzzle;
    }
}