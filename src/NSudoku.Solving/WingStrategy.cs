using System;
using System.Linq;
using NSudoku.Util;

namespace NSudoku.Solving;

public class WingStrategy : IStrategy
{
    private readonly int _size;
    private readonly string _name;

    public WingStrategy(int size, string name)
    {
        _size = size;
        _name = name;
    }

    public StrategyResult Apply(Grid grid)
    {
        // First of all find the pivots

        var pivots = grid.Where(c => c.Candidates.Count == _size);

        foreach (var pivot in pivots) {
            var possibleWings =
                (from candidate in pivot.Candidates
                from wing in grid.GetCellsVisibleTo(pivot)
                where wing.Candidates.Count == 2 && wing.Candidates.Has(candidate)
                group wing by candidate
                into g
                select g)
                .ToList();

            if (possibleWings.Count < pivot.Candidates.Count) {
                continue;
            }

            if (possibleWings.Count > pivot.Candidates.Count) {
                throw new InvalidOperationException("This should not happen");
            }

            var wingSets = possibleWings.Combinations();
            foreach (var wingSet in wingSets) {
                // Find the other values on the wings that are not on the pivot
                // This should be a single value, repeated for each tip.
                // If not, then we don't have a wing
                var others = wingSet.SelectMany(w => w.Candidates)
                    .Where(c => !pivot.Candidates.Has(c))
                    .ToList();
                var digitsOnOthers = others.Distinct().ToList();

                if (others.Count == _size && digitsOnOthers.Count == 1) {
                    var digitToRemove = others[0];

                    var cells = grid.GetCellsVisibleToAll(wingSet.ToArray())
                        .Where(cell => cell.Candidates.Has(digitToRemove))
                        .ToList();

                    if (cells.Any())
                    {
                        foreach (var cell in cells) {
                            cell.Candidates.Remove(digitToRemove);
                        }

                        var pivotCandidateDescription = string.Join(", ", pivot.Candidates);

                        var wingDescriptions =
                            from wing in wingSet
                            select $"{wing.Ref} with candidates {string.Join(",", wing.Candidates)}";

                        var description =
                            $"{_name} with pivot cell at {pivot.Ref} which has candidates {pivotCandidateDescription}. " +
                            $"This sees {string.Join(", ", wingDescriptions)}." +
                            $"These cells see {string.Join(", ", cells.Select(c => c.Ref))} " +
                            $"and the number {digitToRemove} can be removed from those cells.";

                        return new StrategyResult(description, cells);
                    }
                }
            }
        }

        return StrategyResult.Unchanged;
    }

    public override string ToString() => _name;

    public static WingStrategy XYWing = new WingStrategy(2, "XY-Wing");

    public static WingStrategy XYZWing = new WingStrategy(3, "XYZ-Wing");

    public static WingStrategy WXYZWing = new WingStrategy(4, "WXYZ-Wing");
}
