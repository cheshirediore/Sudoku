using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Sudoku.Tests
{
    [TestFixture]
    public class PuzzleTests
    {


        [Test]
        public void Puzzle_Constructor_InitializesGridAndRegions()
        {
            Puzzle _puzzle = new Puzzle();

            Assert.That(_puzzle.CellGrid, Is.Not.Null, "CellGrid should not be null.");
            Assert.That(_puzzle.Regions, Is.Not.Null, "Regions dictionary should not be null.");
            Assert.That(_puzzle.Regions.Count, Is.EqualTo(3), "Regions dictionary should contain 3 region types.");

            foreach (var regionType in Enum.GetValues(typeof(RegionType)))
            {
                Assert.That(_puzzle.Regions.ContainsKey((RegionType)regionType), Is.True, $"Regions should contain key for {regionType}.");
                Assert.That(_puzzle.Regions[(RegionType)regionType].Count, Is.EqualTo(9), $"Each region type should have 9 regions.");
            }
        }

        [Test]
        public void Puzzle_GetRegion_ValidInputs_ReturnsCorrectRegion()
        {
            Puzzle _puzzle = new Puzzle();
            
            var region = _puzzle.GetRegion(RegionType.BLOCK, 0);
            Assert.That(region, Is.Not.Null, "Region should not be null.");
            Assert.That(region.Type, Is.EqualTo(RegionType.BLOCK), "Region type should match.");
        }

        [Test]
        public void Puzzle_GetRegion_InvalidRegionType_ThrowsArgumentOutOfRangeException()
        {
            Puzzle _puzzle = new Puzzle();
            
            Assert.That(() => _puzzle.GetRegion((RegionType)999, 0), Throws.TypeOf<ArgumentOutOfRangeException>(), "Invalid RegionType should throw ArgumentOutOfRangeException.");
        }

        [Test]
        public void Puzzle_GetRegion_InvalidRegionIndex_ThrowsArgumentOutOfRangeException()
        {
            Puzzle _puzzle = new Puzzle();
            
            Assert.That(() => _puzzle.GetRegion(RegionType.BLOCK, -1), Throws.TypeOf<ArgumentOutOfRangeException>(), "Negative region index should throw ArgumentOutOfRangeException.");
            Assert.That(() => _puzzle.GetRegion(RegionType.BLOCK, 10), Throws.TypeOf<ArgumentOutOfRangeException>(), "Out-of-range region index should throw ArgumentOutOfRangeException.");
        }

        [Test]
        public void Puzzle_GetRegionIndex_ValidInputs_ReturnsCorrectIndex()
        {
            int index = Puzzle.GetRegionIndex(0, RegionType.BLOCK);
            Assert.That(index, Is.EqualTo(0), "Region index should be calculated correctly.");

            index = Puzzle.GetRegionIndex(40, RegionType.ROW);
            Assert.That(index, Is.EqualTo(4), "Region index for ROW should be calculated correctly.");

            index = Puzzle.GetRegionIndex(40, RegionType.COLUMN);
            Assert.That(index, Is.EqualTo(4), "Region index for COLUMN should be calculated correctly.");
        }

        [Test]
        public void Puzzle_GetRegionIndex_InvalidRegionType_ThrowsArgumentOutOfRangeException()
        {
            Assert.That(() => Puzzle.GetRegionIndex(0, (RegionType)999), Throws.TypeOf<ArgumentOutOfRangeException>(), "Invalid RegionType should throw ArgumentOutOfRangeException.");
        }

        [Test]
        public void Puzzle_GetRegionIndex_InvalidCellIndex_ThrowsArgumentOutOfRangeException()
        {
            Assert.That(() => Puzzle.GetRegionIndex(-1, RegionType.BLOCK), Throws.TypeOf<ArgumentOutOfRangeException>(), "Negative cell index should throw ArgumentOutOfRangeException.");
            Assert.That(() => Puzzle.GetRegionIndex(100, RegionType.BLOCK), Throws.TypeOf<ArgumentOutOfRangeException>(), "Out-of-range cell index should throw ArgumentOutOfRangeException.");
        }
    }
}