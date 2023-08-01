using System;
using System.Linq;
using System.Net.Sockets;
using NSudoku.Util;

namespace NSudoku.Solving;

public class XYZWingStrategy : IStrategy
{
    const int _size = 3;

    public StrategyResult Apply(Grid grid)
    {
        // First of all find the pivots

        var pivots = grid.Where(c => c.Candidates.Count == _size);

        foreach (var pivot in pivots) {
            var cellsSeenByPivot = grid.GetCellsVisibleTo(pivot)
                .Where(c => !c.HasDigit && c.Candidates.Count == 2)
                .ToList();

            // Possible wings are cells that see the pivot and have both their
            // candidates in the pivot. All wings at this stage have two candidates.

            var possibleWings =
                from wing in cellsSeenByPivot
                where wing.Candidates.All(c => pivot.Candidates.Has(c))
                select wing;

            // We need to find all combinations of two wings, where the wings
            // have different candidates.

            var wingSets = possibleWings.Combinations(2)
                .Where(set => !set[0].Candidates.Equals(set[1].Candidates))
                .ToList();

            foreach (var wingSet in wingSets) {
                // Find the value that is common to both wing sets.
                // This will always be true, if not I'll rail against all the first-born of Egypt.
                var digitToRemove = wingSet[0].Candidates.Intersect(wingSet[1].Candidates).Single();

                // Now we need to find any cells that see both wings and the pivot

                var wingsAndPivot = wingSet.Concat(new[] {pivot}).ToArray();

                var cells = grid.GetCellsVisibleToAll(wingsAndPivot)
                    .Where(c => c.Candidates.Has(digitToRemove))
                    .ToList();

                if (cells.Any()) {
                    foreach (var cell in cells) {
                        cell.Candidates.Remove(digitToRemove);
                    }

                    var description =
                        $"XYZ-wing with pivot cell at {pivot} which sees {string.Join(", ", wingSet)}. " +
                        $"These all see {string.Join(", ", cells)} " +
                        $"and the number {digitToRemove} can be removed from those cells.";

                    return new StrategyResult(description, cells);
                }
            }
        }

        return StrategyResult.Unchanged;
    }

    public override string ToString() => "XYZ-wing";

    public static XYZWingStrategy XYZWing = new();
}
