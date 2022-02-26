using System.Linq;

namespace NSudoku.Constraints;

public class ColumnConstraint : CellConstraint
{
    private readonly byte _column;

    public ColumnConstraint(Grid grid, byte column)
        : base(grid.Where(cell => cell.Ref.Column == column), true)
    {
        _column = column;
    }

    public override string ToString() => $"column {_column}";
}
