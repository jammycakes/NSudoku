using System.Collections.Generic;

namespace NSudoku
{
    public interface IConstraint
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        IEnumerable<Cell> GetCellsVisibleTo(Cell cell);

        /// <summary>
        ///  Applies this constraint to the grid. This means eliminating candidates that are
        ///  disallowed by this constraint for the given digits.
        /// </summary>
        /// <param name="grid"></param>
        void Apply(Grid grid);
    }
}
