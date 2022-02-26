using System.Collections.Generic;
using System.Linq;
using NSudoku.Solving.Objects;
using NSudoku.Solving.Utilities;

namespace NSudoku.Solving
{
    public class IntersectionStrategy : IStrategy
    {
        public StrategyResult Apply(Grid grid)
        {
            /*
             * An intersection occurs if a complete region A overlaps with a unique constraint B,
             * and a particular candidate occurs only in A in the cells that are shared.
             * In this case, the candidate will be removed from all cells in B that are not in A.
             * Note that A must be a complete region, but B need not be.
             */

            var intersections =
                from region1 in grid.Constraints.OfType<ICellConstraint>().Where(r => r.IsRegion())
                from region2 in grid.Constraints.OfType<ICellConstraint>().Where(r => r.Unique)
                let intersection = new Intersection(region1, region2)
                where intersection.Intersect
                      && intersection.CommonCells.Any(c => !c.HasDigit)
                      && intersection.Region1Cells.Any(c => !c.HasDigit)
                      && intersection.Region2Cells.Any(c => !c.HasDigit)
                select intersection;

            foreach (var intersection in intersections) {
                var candidates = intersection.CommonCells
                    .Where(c => !c.HasDigit)
                    .SelectMany(c => c.Candidates);
                foreach (var candidate in candidates) {
                    if (!intersection.Region1Cells.Any(c => c.Candidates.Has(candidate))) {
                        ISet<Cell> changedCells = new HashSet<Cell>();
                        foreach (var cell in intersection.Region2Cells) {
                            if (cell.Candidates.Remove(candidate)) {
                                changedCells.Add(cell);
                            }
                        }

                        if (changedCells.Any()) {
                            var strCells = string.Join
                                (",", changedCells.Select(c => c.Ref.ToString()).OrderBy(c => c));
                            var description =
                                $"Intersection between {intersection.Region1} and {intersection.Region2}. " +
                                $"Value {candidate} removed from cells {strCells}.";

                            return new StrategyResult(description, changedCells);
                        }
                    }
                }
            }

            return StrategyResult.Unchanged;
        }

        public override string ToString() => "Intersection";
    }
}
