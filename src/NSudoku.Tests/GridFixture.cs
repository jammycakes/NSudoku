using System;
using FluentAssertions;
using Xunit;

namespace NSudoku.Tests
{
    public class GridFixture
    {
        [Fact]
        public void CellRefsShouldBeCorrect()
        {
            var grid = new Grid(9);
            for (var row = 1; row <= 9; row++) {
                for (var column = 1; column <= 9; column++) {
                    var cellRef = new CellRef(row, column);
                    grid[cellRef].Ref.Should().Be(cellRef);
                }
            }
        }
    }
}
