using System.Linq;
using FluentAssertions;
using NSudoku.Constraints;
using NSudoku.Strategies.Utilities;
using Xunit;

namespace NSudoku.Tests.Strategies.Utilities
{
    public class FindLockedSetsTests
    {
        [Fact]
        public void FindLockedSet()
        {
            var grid = new Grid(9);
            grid[6, 1].Candidates.Add(4, 9);
            grid[6, 2].Candidates.Add(6, 7);
            grid[6, 3].Candidates.Add(6, 7);
            grid[6, 4].PlaceDigit(8);
            grid[6, 5].Candidates.Add(3, 4, 7, 9);
            grid[6, 6].PlaceDigit(5);
            grid[6, 7].PlaceDigit(2);
            grid[6, 8].PlaceDigit(1);
            grid[6, 9].Candidates.Add(3, 6, 7, 9);

            var constraint = new RowConstraint(grid, 6);
            var lockedSets = constraint.FindLockedSets(2);
            var set = lockedSets.Single();
            set[0].Candidates.Should().BeEquivalentTo((byte)6, (byte)7);
        }
    }
}
