namespace NSudoku;

public interface IStrategy
{
    /// <summary>
    ///  Applies the current strategy to the grid, once.
    /// </summary>
    /// <param name="grid"></param>
    /// <returns>
    ///  A StrategyResult instance with details of what, if anything,
    ///  has changed.
    /// </returns>
    StrategyResult Apply(Grid grid);
}
