using FluentAssertions;
using System.Linq;

using NSudoku.Solving;

using Xunit;

namespace NSudoku.Tests.Strategies;

public class XYZWingTests
{

    [Fact]
    public void ShoudldFindXYZWing()
    {
        /*
         * Grid Wing.50.txt follows up the XY-wing with an XYZ-wing.
         * This being the case, we can reuse this grid to test the XYZ-wing
         * by simply applying the XY-wing first.
         */

        var grid = GridLoader.Load("Wing.50.txt");
        grid.AddDefaultConstraints();
        XYWingStrategy.XYWing.Apply(grid);

        var result = XYZWingStrategy.XYZWing.Apply(grid);

        result.HasChanges.Should().BeTrue();
        result.CellsChanged.Single().Should().Be(grid[3, 2]);
        result.CellsChanged.Single().Candidates.Single().Should().Be(6);
    }
}
