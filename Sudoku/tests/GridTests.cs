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

            for (int y = 0; y < Grid.HEIGHT; y++)
            {
                for (int x = 0; x < Grid.WIDTH; x++)
                {
                    Assert.That(0, Is.EqualTo(grid.GetVertex(x, y)));
                }
            }
        }

        [Test]
        public void Grid_Constructor_GridFromArray_ShouldInitializeCorrectly()
        {
            int[][] values = new int[Grid.HEIGHT][];
            for (int y = 0; y < Grid.HEIGHT; y++)
            {
                values[y] = new int[Grid.WIDTH];
                for (int x = 0; x < Grid.WIDTH; x++)
                {
                    values[y][x] = x + y * Grid.WIDTH;
                }
            }

            var grid = new Grid(values);

            for (int y = 0; y < Grid.HEIGHT; y++)
            {
                for (int x = 0; x < Grid.WIDTH; x++)
                {
                    Assert.That(x + y * Grid.WIDTH, Is.EqualTo(grid.GetVertex(x, y)));
                }
            }
        }

        [Test]
        public void Grid_Constructor_GridFromFile_ShouldInitializeCorrectly()
        {
            string filePath = "/Users/joshuamoore/Development/CSharp/Sudoku/Sudoku/SamplePuzzleSeed.csv";
            var grid = new Grid(filePath);

            Assert.That(grid, Is.Not.Null);
            // Additional assertions can be added based on the file content
        }

        [Test]
        public void Grid_SetVertex_ShouldUpdateValue()
        {
            var grid = new Grid();
            grid.SetVertex(0, 0, 5);

            Assert.That(5, Is.EqualTo(grid.GetVertex(0, 0)));
        }

        [Test]
        public void Grid_SetVertex_ShouldThrowExceptionForInvalidCoordinates()
        {
            var grid = new Grid();

            Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetVertex(-1, 0, 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetVertex(0, -1, 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetVertex(9, 0, 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetVertex(0, 9, 5));
        }

        [Test]
        public void Grid_SetVertex_ShouldNotThrowExceptionForValidCoordinates()
        {
            var grid = new Grid();

            Assert.DoesNotThrow(() => grid.SetVertex(1, 0, 5));
            Assert.DoesNotThrow(() => grid.SetVertex(0, 1, 5));
            Assert.DoesNotThrow(() => grid.SetVertex(8, 0, 5));
            Assert.DoesNotThrow(() => grid.SetVertex(0, 8, 5));
        }

        [Test]
        public void Grid_IsGridConsistent_ShouldReturnTrueForValidGrid()
        {
            var grid = new Grid();
            grid.SetVertex(0, 0, 1);
            grid.SetVertex(1, 0, 2);
            grid.SetVertex(2, 0, 3);

            Assert.That(grid.IsGridConsistent(), Is.True);
        }

        [Test]
        public void Grid_IsGridConsistent_ShouldReturnFalseForInvalidGrid()
        {
            var grid = new Grid();
            grid.SetVertex(0, 0, 1);
            grid.SetVertex(1, 0, 1);

            Assert.That(grid.IsGridConsistent(), Is.False);
        }

        [Test]
        public void Grid_IsGridComplete_ShouldReturnFalseForIncompleteGrid()
        {
            var grid = new Grid();

            Assert.That(grid.IsGridComplete(), Is.False);
        }

        [Test]
        public void Grid_IsGridComplete_ShouldReturnTrueForCompleteGrid()
        {
            var grid = new Grid();

            for (int y = 0; y < Grid.HEIGHT; y++)
            {
                for (int x = 0; x < Grid.WIDTH; x++)
                {
                    grid.SetVertex(x, y, 1);
                }
            }

            Assert.That(grid.IsGridComplete(), Is.True);
        }
    }
}