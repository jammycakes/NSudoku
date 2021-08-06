using FluentAssertions;
using NSudoku.Solving;
using NSudoku.Strategies;
using Xunit;

namespace NSudoku.Tests.Strategies
{
    public class LockedSetsTests
    {
        [Fact]
        public void LockedSetStrategyShouldPickUpOnAllNakedPairs()
        {
            var board =
                "000000021800500000000060000030102000500000840000000000000780500620000000004000000";
            var grid = Grid.Parse(board);
            var solver = new Solver()
                .AddSingleStrategies(true);
            solver.Solve(grid);

            /*
             * This solution path sets up a grid with four subsequent naked pairs:
             *
             * box 4 in r4c1: [49] and r6c1: [49]. Values 4,9 removed from cells r6c2,r6c3.
             * row 6 in r6c2: [67] and r6c3: [67]. Values 6,7 removed from cells r6c5,r6c9.
             * row 8 in r8c4: [39] and r8c6: [39]. Values 3,9 removed from cells r8c7,r8c8,r8c9.
             * column 9 in r6c9: [39] and r9c9: [39]. Values 3,9 removed from cells r2c9,r3c9,r4c9,r5c9.
             *
             * Naked pair strategy should pick up on all of them.
             */

            var strategy = LockedSetStrategy.NakedPair;

            for (var a = 1; a <= 4; a++) {
                var result = strategy.Apply(grid);
                result.HasChanges.Should().BeTrue();
            }
        }
    }
}
