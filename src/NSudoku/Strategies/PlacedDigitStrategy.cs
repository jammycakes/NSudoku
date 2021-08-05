using System.Collections.Generic;
using System.Linq;

namespace NSudoku.Strategies
{
    /// <summary>
    ///  This is the abstract base class for any strategy that might result
    ///  in a digit being placed. These need to be followed by a sweep of
    ///  the grid to remove any candidates that are no longer valid.
    ///
    ///  Normally there are only two: hidden singles and naked singles.
    ///  Other strategies will only result in the number of candidates
    ///  in a cell being reduced to one.
    /// </summary>
    public abstract class PlacedDigitStrategy : IStrategy
    {
        public abstract StrategyResult Apply(Grid grid);

        protected IEnumerable<Cell> Sweep(Grid grid)
        {
            return
                from constraint in grid.Constraints
                from cell in constraint.Apply(grid)
                select cell;
        }

    }
}
