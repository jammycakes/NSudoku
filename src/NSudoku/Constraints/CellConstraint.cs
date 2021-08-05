using System;
using System.Collections.Generic;
using System.Linq;

namespace NSudoku.Constraints
{
    public abstract class CellConstraint : ICellConstraint
    {
        protected CellConstraint(IEnumerable<Cell> cells, bool unique)
        {
            Cells = cells.ToList().AsReadOnly();
            Unique = unique;
            Grid = Cells.Select(c => c.Grid).Distinct().Single();
        }

        public IEnumerable<Cell> GetCellsVisibleTo(Cell cell)
        {
            if (Cells.Contains(cell)) {
                return Cells.Where(other => other != cell);
            }
            else {
                return Enumerable.Empty<Cell>();
            }
        }

        public virtual IEnumerable<Cell> Apply()
        {
            var changed = new HashSet<Cell>();
            if (Unique) {
                foreach (var cell in Grid.Where(c => c.HasDigit)) {
                    var visibleCells = GetCellsVisibleTo(cell);
                    foreach (var seen in visibleCells) {
                        if (seen.Candidates.Remove(cell.Digit.Value)) {
                            changed.Add(seen);
                        };
                    }
                }
            }

            return changed;
        }

        public ICollection<Cell> Cells { get; }

        public Grid Grid { get; }

        public bool Unique { get; }
    }
}
