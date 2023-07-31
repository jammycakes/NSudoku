using System.Collections.Generic;

namespace NSudoku;

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
    /// <param name="grid">The grid.</param>
    /// <returns>The cells that have been changed by applying this constraint.</returns>
    IEnumerable<Cell> Apply();

    /// <summary>
    ///  The grid to which this constraint is applied.
    /// </summary>
    Grid Grid { get; }
}
