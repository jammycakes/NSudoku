using System.Linq;
using FluentAssertions;
using NSudoku.Solving;
using Xunit;

namespace NSudoku.Tests.Strategies;

public class WingTests
{
    /*
     * TODO: Grid Wing.50.txt has an XY-wing with pivot cell at R7C2 (79)
     * and tips at R9C3 (67) and R3C2 (69). This should allow us to eliminate
     * the 6 from R2C3 (567).
     */

    [Fact]
    public void ShouldFindXYWing()
    {
        var grid = GridLoader.Load("Wing.50.txt");
        grid.AddDefaultConstraints();
        var result = WingStrategy.XYWing.Apply(grid);
        result.HasChanges.Should().BeTrue();
        result.CellsChanged.Single().Should().Be(grid[2, 3]);
        result.CellsChanged.Single().Candidates.Should().BeEquivalentTo(new byte[] {5, 7});
    }
}
