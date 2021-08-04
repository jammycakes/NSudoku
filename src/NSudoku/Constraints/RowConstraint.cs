using System.Collections.Generic;
using System.Linq;

namespace NSudoku.Constraints
{
    public class RowConstraint : CellConstraint
    {
        private readonly byte _row;

        public RowConstraint(Grid grid, byte row)
            : base(grid.Where(cell => cell.Ref.Row == row), true)
        {
            _row = row;
        }

        public override string ToString() => $"row {_row}";
    }
}
