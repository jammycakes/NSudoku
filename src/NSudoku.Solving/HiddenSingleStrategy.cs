using System.Linq;

namespace NSudoku.Strategies
{
    /* ====== Hidden singles ====== */

    /// <summary>
    ///  A hidden single is the only cell in a region of the grid where a particular
    ///  digit can be placed. Despite its name, it is one of the easiest sudoku
    ///  solving techniques, especially when it occurs in a box rather than a row or column.
    ///
    ///  Because hidden singles are easier to spot in boxes than in rows or columns,
    ///  they are counted separately. To do this, we will make this strategy generic.
    ///  TRegion will be either DefaultRegionConstraint or ICellConstraint accordingly.
    /// </summary>
    /// <typeparam name="TRegion"></typeparam>
    public class HiddenSingleStrategy<TRegion> : PlacedDigitStrategy
        where TRegion: ICellConstraint
    {
        public override StrategyResult Apply(Grid grid)
        {
            var (cell, digit, region) = FindFirst(grid);
            if (cell != null) {
                cell.PlaceDigit(digit);
                var cells = new[] {cell}.Concat(Sweep(grid));
                return new StrategyResult
                    ($"Hidden single in {region}: {cell.Ref}={cell.Digit}", cells);
            }

            return StrategyResult.Unchanged;
        }

        private (Cell, byte, IConstraint) FindFirst(Grid grid)
        {
            foreach (var region in grid.Constraints.OfType<TRegion>()) {
                if (region.Unique && region.Cells.Count == grid.Size) {
                    for (byte digit = 1; digit <= grid.Size; digit++) {
                        var matchingCells =
                            region.Cells.Where(c => c.Candidates.Has(digit) && !c.HasDigit);
                        if (matchingCells.Count() == 1) {
                            return (matchingCells.Single(), digit, region);
                        }
                    }
                }
            }

            return (null, 0, null);
        }

        public override string ToString() => $"Hidden single ({typeof(TRegion).Name})";
    }
}
