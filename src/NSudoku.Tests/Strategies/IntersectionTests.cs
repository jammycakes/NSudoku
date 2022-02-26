using FluentAssertions;
using NSudoku.Strategies;
using Xunit;

namespace NSudoku.Tests.Strategies;

public class IntersectionTests
{
    [Fact]
    public void IntersectionShouldRemoveCellsFromRow()
    {
        var grid = new Grid(9).AddDefaultConstraints();

        for (byte row = 1; row <= 9; row++)
        for (byte column = 1; column <= 9; column++) {
            if (row <= 4 || row > 6 ||
                (row == 5 && (column <= 3 || column > 6))) {
                grid[row, column].Candidates.Add(3);
            }
        }

        var strategy = new IntersectionStrategy();
        var result = strategy.Apply(grid);
        result.Should().NotBeNull();
        result.CellsChanged.Count.Should().Be(6);

        for (byte i = 1; i <= 9; i++) {
            grid[4, i].Candidates.Has(3).Should().Be(i >= 4 && i <= 6);
            grid[5, i].Candidates.Has(3).Should().Be(i < 4 || i > 6);
        }
    }
}
