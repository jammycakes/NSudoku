using System.Collections.Generic;
using NSudoku.Constraints;

namespace NSudoku.Solving.Solving;

public class Solver
{
    private IList<IStrategy> _strategies = new List<IStrategy>();

    public Solver Add(IStrategy strategy)
    {
        _strategies.Add(strategy);
        return this;
    }

    public Solver Add<TStrategy>() where TStrategy : IStrategy, new() => Add(new TStrategy());

    /// <summary>
    ///  Adds the strategies to handle naked and hidden singles.
    /// </summary>
    /// <remarks>
    ///  As a performance optimisation, we can tweak the order in which we tackle
    ///  things here. Optimal performance comes from prioritising naked singles
    ///  over hidden singles, sweeping the grid for all naked singles at once, and
    ///  not distinguishing between grid-based and row-based hidden singles.
    ///  However, this will reduce the accuracy of any grading algorithm that we
    ///  use, as hidden singles are easier for a human to spot than naked singles,
    ///  whereas for a computer it is the other way round.
    /// </remarks>
    /// <param name="forGrading"></param>
    /// <returns></returns>
    public Solver AddSingleStrategies(bool forGrading)
    {
        if (forGrading) {
            return Add<HiddenSingleStrategy<DefaultRegionConstraint>>()
                .Add<HiddenSingleStrategy<ICellConstraint>>()
                .Add(new NakedSingleStrategy(false));
        }
        else {
            return Add(new NakedSingleStrategy(true))
                .Add<HiddenSingleStrategy<ICellConstraint>>();
        }
    }

    public Solution Solve(Grid grid)
    {
        var solution = new Solution(grid, this._strategies);
        solution.Solve();
        return solution;
    }
}
