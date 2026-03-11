using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Sudoku;

namespace SudokuTests
{
    [TestFixture]
    public class CellTests
    {
        [Test]
        public void Cell_RemoveCandidate_RemovesSpecifiedCandidate()
        {
            // Arrange
            var cell = new Cell();

            // Act
            bool result = cell.RemoveCandidate(5);

            // Assert
            Assert.That(result, Is.True); // Candidate should be removed.
            Assert.That(cell.Candidates.Contains(5), Is.False); // Candidates list should not contain the removed candidate.
        }

        [Test]
        public void Cell_RemoveCandidate_ReturnsFalse_WhenCandidateNotPresent()
        {
            // Arrange
            var cell = new Cell();
            cell.RemoveCandidate(5); // Remove 5 first

            // Act
            bool result = cell.RemoveCandidate(5);

            // Assert
            Assert.That(result, Is.False); // Should return false when candidate is not present.
        }

        [Test]
        public void Cell_RemoveCandidate_DoesNotAffectOtherCandidates()
        {
            // Arrange
            var cell = new Cell();

            // Act
            cell.RemoveCandidate(5);

            // Assert
            CollectionAssert.AreEquivalent(new List<int> { 1, 2, 3, 4, 6, 7, 8, 9 }, cell.Candidates,
                "Only the specified candidate should be removed.");
        }
    }
}