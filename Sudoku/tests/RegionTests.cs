using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Sudoku.Tests;

[TestFixture]
public class RegionTests
{
    [Test]
    public void Region_Constructor_ValidRegionType_ShouldInitializeRegion()
    {
        var region = new Region(RegionType.BLOCK);

        Assert.That(region.Type, Is.EqualTo(RegionType.BLOCK));
        Assert.That(region.Cells, Is.Not.Null);
        Assert.That(region.Cells.Count, Is.EqualTo(0));
    }

    [Test]
    public void Region_Constructor_InvalidRegionType_ShouldThrowArgumentException()
    {
        Assert.That(() => new Region((RegionType)999), Throws.ArgumentException);
    }

    [Test]
    public void Region_AddCell_ValidCell_ShouldAddCellToRegion()
    {
        var region = new Region(RegionType.ROW);
        var cell = new Cell();

        var result = region.AddCell(cell);

        Assert.That(result, Is.True);
        Assert.That(region.Cells.Count, Is.EqualTo(1));
        Assert.That(region.Cells[0], Is.SameAs(cell));
    }

    [Test]
    public void Region_AddCell_ExceedCapacity_ShouldThrowInvalidOperationException()
    {
        var region = new Region(RegionType.BLOCK);

        for (int i = 0; i < 9; i++)
        {
            region.AddCell(new Cell());
        }

        Assert.That(() => region.AddCell(new Cell()), Throws.InvalidOperationException);
    }
}