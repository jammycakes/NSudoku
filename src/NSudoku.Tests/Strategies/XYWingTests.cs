using System.Linq;
using FluentAssertions;
using NSudoku.Solving;
using Xunit;

namespace NSudoku.Tests.Strategies;

public class XYWingTests
{
    [Fact]
    public void ShouldFindXYWing()
    {
        var grid = GridLoader.Load("Wing.50.txt");
        grid.AddDefaultConstraints();
        var result = XYWingStrategy.XYWing.Apply(grid);
        result.HasChanges.Should().BeTrue();
        result.CellsChanged.Single().Should().Be(grid[2, 3]);
        result.CellsChanged.Single().Candidates.Should().BeEquivalentTo(new byte[] {5, 7});
    }
}
