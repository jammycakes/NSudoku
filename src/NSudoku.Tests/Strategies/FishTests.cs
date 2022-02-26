using System.Linq;
using FluentAssertions;
using NSudoku.Solving;
using Xunit;

namespace NSudoku.Tests.Strategies;

public class FishTests
{
    [Fact]
    public void XWingInRowsShouldEliminateColumns()
    {
        var grid = new Grid(9).AddDefaultConstraints();
        foreach (var cell in grid) {
            if (cell.Ref.Row != 3 && cell.Ref.Row != 6) {
                cell.Candidates.Add(3);
            }
        }

        grid[3, 3].Candidates.Add(3);
        grid[3, 6].Candidates.Add(3);
        grid[6, 3].Candidates.Add(3);
        grid[6, 6].Candidates.Add(3);

        var result = FishStrategy.XWing.Apply(grid);

        result.HasChanges.Should().BeTrue();
        result.CellsChanged.Where(cell => cell.Ref.Column != 3 && cell.Ref.Column != 6)
            .Should().BeEmpty();

        for (byte i = 1; i <= 9; i++)
        for (byte j = 1; j <= 9; j++) {
            var shouldContainCandidate =
                !(i == 3 || i == 6) ^ (j == 3 || j == 6);
            grid[i, j].Candidates.Has(3).Should().Be(shouldContainCandidate);
        }
    }

    [Fact]
    public void SwordfishInColumnsShouldEliminateRows()
    {
        var grid = new Grid(9).AddDefaultConstraints();
        foreach (var cell in grid) {
            if (cell.Ref.Column % 3 != 0) {
                cell.Candidates.Add(3);
            }
        }

        for (byte i = 3; i <= 9; i+= 3)
        for (byte j = 3; j <= 9; j += 3) {
            if (i != j) {
                grid[i, j].Candidates.Add(3);
            }
        }

        var result = FishStrategy.Swordfish.Apply(grid);

        result.HasChanges.Should().BeTrue();
        result.CellsChanged.Where(cell => cell.Ref.Row % 3 != 0)
            .Should().BeEmpty();

        for (byte i = 1; i <= 9; i++)
        for (byte j = 1; j <= 9; j++) {
            var shouldContainCandidate = (i % 3 != 0 && j % 3 != 0)
                                         || (i % 3 == 0 && j % 3 == 0 && i != j);
            grid[i, j].Candidates.Has(3).Should().Be(shouldContainCandidate, $"Expected r{{0}}c{{1}} should be {shouldContainCandidate}", i, j);
        }
    }
}
