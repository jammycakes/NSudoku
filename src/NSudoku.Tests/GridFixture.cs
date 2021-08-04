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
            for (byte row = 1; row <= 9; row++) {
                for (byte column = 1; column <= 9; column++) {
                    grid[row, column].Ref.Row.Should().Be(row);
                    grid[row, column].Ref.Column.Should().Be(column);
                }
            }
        }
    }
}
