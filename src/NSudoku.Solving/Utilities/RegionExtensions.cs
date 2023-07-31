using System.Collections.Generic;
using System.Linq;
using NSudoku.Util;

namespace NSudoku.Solving.Utilities;

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

    /// <summary>
    ///  Finds all the locked sets (aka naked sets) of a given size in a region.
    /// </summary>
    /// <remarks>
    ///  A locked set is a set of n cells in a given region which contain n or fewer
    ///  candidates between them.
    /// </remarks>
    /// <param name="constraint">The region being examined.</param>
    /// <param name="size">The size of the locked sets to find.</param>
    /// <returns></returns>

    public static IEnumerable<Cell[]> FindLockedSets(this ICellConstraint constraint, int size, bool hidden)
    {
        if (!constraint.Unique) {
            yield break;
        }

        var availableCells = constraint.Cells.Where(cell => !cell.HasDigit);
        var cellCount = availableCells.Count();

        if (hidden) {
            size = cellCount - size;
        }

        var cellsToConsider = availableCells.Where(cell => cell.Candidates.Count <= size);

        var combinations = cellsToConsider.Combinations(size).ToList();
        foreach (var combination in combinations) {
            var candidates = combination.SelectMany(c => c.Candidates).Distinct().ToList();
            if (candidates.Count <= size) {
                yield return combination.ToArray();
            }
        }
    }
}
