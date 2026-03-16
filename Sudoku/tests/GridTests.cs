using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Sudoku.Tests
{
    [TestFixture]
    public class GridTests
    {
        [Test]
        public void Grid_Constructor_DefaultGrid_ShouldInitializeEmptyGrid()
        {
            var grid = new Grid();

            for (int index = 0; index < Grid.SIZE; index++)
            {
                Assert.DoesNotThrow(() => {Cell c = grid.Vertices[index];}); // Verify that the cell exists
                Assert.That(null != grid.Vertices[index]); // Verify that it isn't null
            }
        }

        [Test]
        public void Grid_CoordinatesToIndex_ShouldReturnCorrectIndex()
        {
            /*
                      0  1  2    3  4  5    6  7  8

                0     0  1  2 |  3  4  5 |  6  7  8
                1     9 10 11 | 12 13 14 | 15 16 17
                2    18 19 20 | 21 22 23 | 24 25 26
                    ------------------------------
                3    27 28 29 | 30 31 32 | 33 34 35
                4    36 37 38 | 39 40 41 | 42 43 44
                5    45 46 47 | 48 49 50 | 51 52 53
                    ------------------------------
                6    54 55 56 | 57 58 59 | 60 61 62
                7    63 64 65 | 66 67 68 | 69 70 71
                8    72 73 74 | 75 76 77 | 78 79 80
            */
            Assert.That(0,  Is.EqualTo(Grid.CoordinatesToIndex(0, 0)));
            Assert.That(11, Is.EqualTo(Grid.CoordinatesToIndex(2, 1)));
            Assert.That(52, Is.EqualTo(Grid.CoordinatesToIndex(7, 5)));
            Assert.That(44, Is.EqualTo(Grid.CoordinatesToIndex(8, 4)));
            Assert.That(40, Is.EqualTo(Grid.CoordinatesToIndex(4, 4)));
            Assert.That(8,  Is.EqualTo(Grid.CoordinatesToIndex(8, 0)));
            Assert.That(72, Is.EqualTo(Grid.CoordinatesToIndex(0, 8)));
            Assert.That(80, Is.EqualTo(Grid.CoordinatesToIndex(8, 8)));
        }


        [Test]
        public void Grid_GetVertex_ShouldThrowExceptionForInvalidCoordinates()
        {
            var grid = new Grid();

            Assert.Throws<ArgumentOutOfRangeException>(() => grid.GetVertex(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.GetVertex(0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.GetVertex(Grid.SIZE, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.GetVertex(0, Grid.SIZE));
        }

        [Test]
        public void Grid_GetVertex_ShouldNotThrowExceptionForValidCoordinates()
        {
            var grid = new Grid();

            // With Index
            Assert.DoesNotThrow(() => grid.GetVertex(0));
            Assert.DoesNotThrow(() => grid.GetVertex(1));
            Assert.DoesNotThrow(() => grid.GetVertex(9));
            Assert.DoesNotThrow(() => grid.GetVertex(8));
            Assert.DoesNotThrow(() => grid.GetVertex(72));
            Assert.DoesNotThrow(() => grid.GetVertex(80));

            // With Coordinates
            Assert.DoesNotThrow(() => grid.GetVertex(0, 0));
            Assert.DoesNotThrow(() => grid.GetVertex(1, 0));
            Assert.DoesNotThrow(() => grid.GetVertex(0, 1));
            Assert.DoesNotThrow(() => grid.GetVertex(8, 0));
            Assert.DoesNotThrow(() => grid.GetVertex(0, 8));
            Assert.DoesNotThrow(() => grid.GetVertex(8, 8));
        }

        [Test]
        public void Grid_GetVertex_ShouldThrowExceptionForInvalidIndex()
        {
            var grid = new Grid();

            Assert.Throws<ArgumentOutOfRangeException>(() => grid.GetVertex(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.GetVertex(81));
        }

        [Test]
        public void Grid_GetVertex_ShouldNotThrowExceptionForValidIndex()
        {
            var grid = new Grid();

            Assert.DoesNotThrow(() => grid.GetVertex(0));
            Assert.DoesNotThrow(() => grid.GetVertex(1));
            Assert.DoesNotThrow(() => grid.GetVertex(56));
            Assert.DoesNotThrow(() => grid.GetVertex(80));
        }

        // [Test]
        // public void Grid_Constructor_GridFromArray_ShouldInitializeCorrectly()
        // {
        //     int[][] values = new int[Sudoku.Deprecated.Grid.HEIGHT][];
        //     for (int y = 0; y < Sudoku.Deprecated.Grid.HEIGHT; y++)
        //     {
        //         values[y] = new int[Sudoku.Deprecated.Grid.WIDTH];
        //         for (int x = 0; x < Sudoku.Deprecated.Grid.WIDTH; x++)
        //         {
        //             values[y][x] = x + y * Sudoku.Deprecated.Grid.WIDTH;
        //         }
        //     }

        //     var grid = new Sudoku.Deprecated.Grid(values);

        //     for (int y = 0; y < Sudoku.Deprecated.Grid.HEIGHT; y++)
        //     {
        //         for (int x = 0; x < Sudoku.Deprecated.Grid.WIDTH; x++)
        //         {
        //             Assert.That(x + y * Sudoku.Deprecated.Grid.WIDTH, Is.EqualTo(grid.GetVertex(x, y)));
        //         }
        //     }
        // }

        // [Test]
        // public void Grid_Constructor_GridFromFile_ShouldInitializeCorrectly()
        // {
        //     string filePath = "/Users/joshuamoore/Development/CSharp/Sudoku/Sudoku/SamplePuzzleSeed.csv";
        //     var grid = new Sudoku.Deprecated.Grid(filePath);

        //     Assert.That(grid, Is.Not.Null);
        //     // Additional assertions can be added based on the file content
        // }

        // [Test]
        // public void Grid_SetVertex_ShouldUpdateValue()
        // {
        //     var grid = new Sudoku.Deprecated.Grid();
        //     grid.SetVertex(0, 0, 5);

        //     Assert.That(5, Is.EqualTo(grid.GetVertex(0, 0)));
        // }

        // [Test]
        // public void Grid_SetVertex_ShouldThrowExceptionForInvalidCoordinates()
        // {
        //     var grid = new Sudoku.Deprecated.Grid();

        //     Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetVertex(-1, 0, 5));
        //     Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetVertex(0, -1, 5));
        //     Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetVertex(9, 0, 5));
        //     Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetVertex(0, 9, 5));
        // }

        // [Test]
        // public void Grid_SetVertex_ShouldNotThrowExceptionForValidCoordinates()
        // {
        //     var grid = new Sudoku.Deprecated.Grid();

        //     Assert.DoesNotThrow(() => grid.SetVertex(1, 0, 5));
        //     Assert.DoesNotThrow(() => grid.SetVertex(0, 1, 5));
        //     Assert.DoesNotThrow(() => grid.SetVertex(8, 0, 5));
        //     Assert.DoesNotThrow(() => grid.SetVertex(0, 8, 5));
        // }

        // [Test]
        // public void Grid_IsGridConsistent_ShouldReturnTrueForValidGrid()
        // {
        //     var grid = new Sudoku.Deprecated.Grid();
        //     grid.SetVertex(0, 0, 1);
        //     grid.SetVertex(1, 0, 2);
        //     grid.SetVertex(2, 0, 3);

        //     Assert.That(grid.IsGridConsistent(), Is.True);
        // }

        // [Test]
        // public void Grid_IsGridConsistent_ShouldReturnFalseForInvalidGrid()
        // {
        //     var grid = new Sudoku.Deprecated.Grid();
        //     grid.SetVertex(0, 0, 1);
        //     grid.SetVertex(1, 0, 1);

        //     Assert.That(grid.IsGridConsistent(), Is.False);
        // }

        // [Test]
        // public void Grid_IsGridComplete_ShouldReturnFalseForIncompleteGrid()
        // {
        //     var grid = new Sudoku.Deprecated.Grid();

        //     Assert.That(grid.IsGridComplete(), Is.False);
        // }

        // [Test]
        // public void Grid_IsGridComplete_ShouldReturnTrueForCompleteGrid()
        // {
        //     var grid = new Sudoku.Deprecated.Grid();

        //     for (int y = 0; y < Sudoku.Deprecated.Grid.HEIGHT; y++)
        //     {
        //         for (int x = 0; x < Sudoku.Deprecated.Grid.WIDTH; x++)
        //         {
        //             grid.SetVertex(x, y, 1);
        //         }
        //     }

        //     Assert.That(grid.IsGridComplete(), Is.True);
        // }
    }
}