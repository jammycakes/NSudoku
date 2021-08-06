using System.Collections.Generic;
using System.Linq;

namespace NSudoku.Strategies
{
    public class NakedPairStrategy : IStrategy
    {
        public StrategyResult Apply(Grid grid)
        {
            foreach (var constraint in grid.Constraints.OfType<ICellConstraint>()) {
                if (constraint.Unique) {
                    var cells = constraint.Cells;
                    var pair = FindPair(cells);
                    if (pair != null) {
                        var removedCells = RemoveOthers(cells, pair, constraint);
                        if (removedCells.Any()) {
                            var candidatesAsString = string.Join(",", pair[0].Candidates);
                            var othersAsString = string.Join(",", removedCells.Select(c => c.Ref));
                            return new StrategyResult(
                                $"Naked pair in {constraint} in {pair[0]} and {pair[1]}. " +
                                $"Values {candidatesAsString} removed from cells {othersAsString}.",
                                removedCells);
                        }
                    }
                }
            }

            return StrategyResult.Unchanged;
        }

        private Cell[] FindPair(ICollection<Cell> cells)
        {
            var twoCandidateCells = cells.Where(c => c.Candidates.Count == 2);
            foreach (var cell in twoCandidateCells) {
                var matching = twoCandidateCells.FirstOrDefault
                        (c => c != cell && c.Candidates.Equals(cell.Candidates));
                if (matching != null) {
                    return new[] {cell, matching};
                }
            }

            return null;
        }

        private IEnumerable<Cell> RemoveOthers(ICollection<Cell> cells, Cell[] pair, IConstraint constraint)
        {
            var changed = new HashSet<Cell>();
            foreach (var cell in cells.Except(pair)) {
                foreach (var candidate in pair[0].Candidates) {
                    if (cell.Candidates.Remove(candidate)) {
                        changed.Add(cell);
                    }
                }
            }

            return changed;
        }

        public override string ToString() => "Naked pair";
    }
}
