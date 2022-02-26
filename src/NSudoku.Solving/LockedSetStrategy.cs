using System.Collections.Generic;
using System.Linq;
using NSudoku.Strategies.Utilities;

namespace NSudoku.Strategies
{
    public class LockedSetStrategy : IStrategy
    {
        private readonly int _size;
        private readonly string _description;
        private readonly bool _hidden;

        public LockedSetStrategy(int size, string description, bool hidden)
        {
            _size = size;
            _description = description;
            _hidden = hidden;
        }

        public StrategyResult Apply(Grid grid)
        {
            var lockedSets =
                from constraint in grid.Constraints.OfType<ICellConstraint>()
                where constraint.Unique
                from lockedSet in constraint.FindLockedSets(_size, _hidden)
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
                    var description = _hidden
                        ? $"{this._description} in {set.constraint} in {othersAsString}. " +
                          $"Values {candidatesAsString} removed."
                        : $"{this._description} in {set.constraint} in {lockedSetAsString}. " +
                          $"Values {candidatesAsString} removed from cells {othersAsString}.";
                    return new StrategyResult(description, changed);
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
            = new LockedSetStrategy(2, "Naked pair", false);

        public static LockedSetStrategy NakedTriple { get; }
            = new LockedSetStrategy(3, "Naked triple", false);

        public static LockedSetStrategy NakedQuad { get; }
            = new LockedSetStrategy(4, "Naked quad", false);

        public static LockedSetStrategy NakedQuintuple { get; }
            = new LockedSetStrategy(5, "Naked quintuple", false);

        public static LockedSetStrategy NakedSextuple { get; }
            = new LockedSetStrategy(6, "Naked sextuple", false);

        public static LockedSetStrategy NakedSeptuple { get; }
            = new LockedSetStrategy(7, "Naked septuple", false);

        public static LockedSetStrategy NakedOctuple { get; }
            = new LockedSetStrategy(8, "Naked octuple", false);

        public static LockedSetStrategy HiddenPair { get; }
            = new LockedSetStrategy(2, "Hidden pair", true);

        public static LockedSetStrategy HiddenTriple { get; }
            = new LockedSetStrategy(3, "Hidden triple", true);

        public static LockedSetStrategy HiddenQuad { get; }
            = new LockedSetStrategy(4, "Hidden quad", true);
    }
}
