using System;
using System.Collections.Generic;
using System.Linq;

namespace NSudoku.Strategies
{
    /// <summary>
    ///  A naked single is a cell that contains only one candidate.
    ///  Computationally, naked singles are the easiest to deal with since we
    ///  do not need to consider any other cells. However, they are harder
    ///  to spot when solving manually, so they need to be counted separately
    ///  when reporting on difficulty.
    /// </summary>
    public class NakedSingleStrategy : PlacedDigitStrategy
    {
        private readonly bool _sweep;

        public NakedSingleStrategy(bool sweep)
        {
            _sweep = sweep;
        }

        public override StrategyResult Apply(Grid grid)
        {
            var placedDigits = PlaceDigits(grid);
            if (placedDigits.Any()) {
                var description = GetDescription(placedDigits);
                var sweptCells = Sweep(grid);
                return new StrategyResult(description, placedDigits.Concat(sweptCells));
            }
            else {
                return StrategyResult.Unchanged;
            }
        }

        private IEnumerable<Cell> PlaceDigits(Grid grid)
        {
            IEnumerable<Cell> cells = grid.Where(c => !c.HasDigit && c.Candidates.Count == 1);
            if (!_sweep) {
                cells = cells.Take(1);
            }

            cells = cells.ToList();
            foreach (var cell in cells) {
                cell.PlaceDigit(cell.Candidates.Single());
            }

            return cells;
        }

        private string GetDescription(IEnumerable<Cell> placedDigits)
        {
            var cells = placedDigits.ToList();
            var description = String.Join(", ", cells.Select(c => $"{c.Ref}={c.Digit}"));
            return (cells.Count > 1 ? "Naked singles: " : "Naked single: ") + description;
        }

        public override string ToString() => "Naked single";
    }
}
