using System.Collections.Generic;

namespace NSudoku
{
    public interface ICellConstraint : IConstraint
    {
        ICollection<Cell> Cells { get; }

        bool Unique { get; }
    }
}
