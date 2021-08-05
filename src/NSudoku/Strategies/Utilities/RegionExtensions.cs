using System.Collections.Generic;
using System.Linq;

namespace NSudoku.Strategies.Utilities
{
    public static class RegionExtensions
    {
        /// <summary>
        ///  Gets a value indicating whether a constraint is a region.
        ///  A region is a unique constraint whose size equals the one dimensional
        ///  size of the grid, and which will therefore contain all the possible numbers
        ///  in the grid.
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>

        public static bool IsRegion(this ICellConstraint constraint)
            => constraint.Unique && constraint.Cells.Count == constraint.Grid.Size;

        /// <summary>
        ///  Gets the cells intersecting two regions.
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="other"></param>
        /// <returns></returns>

        public static IEnumerable<Cell> GetIntersectingCells
            (this ICellConstraint constraint, ICellConstraint other)
        {
            return constraint.Cells.Intersect(other.Cells).ToList();
        }
    }
}
