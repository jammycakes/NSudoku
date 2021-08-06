using System.Collections.Generic;
using System.Linq;
using NSudoku.Constraints;
using NSudoku.Strategies.Utilities;

namespace NSudoku.Strategies
{
    public class LockedSetStrategy : IStrategy
    {
        private readonly int _size;
        private readonly string _description;

        public LockedSetStrategy(int size, string description)
        {
            _size = size;
            _description = description;
        }

        public StrategyResult Apply(Grid grid)
        {
            var lockedSets =
                from constraint in grid.Constraints.OfType<ICellConstraint>()
                where constraint.Unique
                from lockedSet in constraint.FindLockedSets(_size)
                select new {lockedSet, constraint};

            foreach (var set in lockedSets) {
                var candidates = set.lockedSet.SelectMany(cell => cell.Candidates)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

                var changed = RemoveOthers(set.constraint.Cells.Except(set.lockedSet), candidates)
                    .OrderBy(c => c.ToString());
                if (changed.Any()) {
                    var candidatesAsString = string.Join(",", candidates);
                    var othersAsString = string.Join(",", changed.Select(c => c.Ref));
                    var lockedSetAsString = string.Join(",", set.lockedSet.Select(c => c.Ref));
                    return new StrategyResult(
                        $"{this._description} in {set.constraint} in {lockedSetAsString}. " +
                        $"Values {candidatesAsString} removed from cells {othersAsString}.",
                        changed);
                }
            }

            return StrategyResult.Unchanged;
        }

        private IEnumerable<Cell> RemoveOthers(IEnumerable<Cell> cells, IEnumerable<byte> candidates)
        {
            var changed = new HashSet<Cell>();
            foreach (var cell in cells) {
                foreach (var candidate in candidates) {
                    if (cell.Candidates.Remove(candidate)) {
                        changed.Add(cell);
                    }
                }
            }

            return changed;
        }

        public override string ToString() => _description;

        public static LockedSetStrategy NakedPair { get; }
            = new LockedSetStrategy(2, "Naked pair");

        public static LockedSetStrategy NakedTriple { get; }
            = new LockedSetStrategy(3, "Naked triple");

        public static LockedSetStrategy NakedQuad { get; }
            = new LockedSetStrategy(4, "Naked quad");

        public static LockedSetStrategy NakedQuintuple { get; }
            = new LockedSetStrategy(5, "Naked quintuple");

        public static LockedSetStrategy NakedSextuple { get; }
            = new LockedSetStrategy(6, "Naked sextuple");

        public static LockedSetStrategy NakedSeptuple { get; }
            = new LockedSetStrategy(7, "Naked septuple");

        public static LockedSetStrategy NakedOctuple { get; }
            = new LockedSetStrategy(8, "Naked octuple");
    }
}
