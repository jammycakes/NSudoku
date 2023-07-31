using FluentAssertions;
using NSudoku.Solving;
using NSudoku.Solving.Solving;
using Xunit;

namespace NSudoku.Tests.Solving;

public class SolverFixture
{
    [Fact]
    public void SolverShouldCompleteRoyle1()
    {
        string royle1 =
            "000000010400000000020000000000050407008000300001090000300400200050100000000806000";
        var grid = Grid.Parse(royle1);
        var solution = new Solver()
            .AddSingleStrategies(true)
            .Solve(grid);
        grid.CountUnplacedDigits().Should().Be(0);
    }
}
