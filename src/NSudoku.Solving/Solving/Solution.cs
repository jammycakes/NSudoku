using System.Collections.Generic;
using System.Linq;

namespace NSudoku.Solving.Solving;

public class Solution
{
    private ISet<IStrategy> _usedStrategies = new HashSet<IStrategy>();

    public Grid Grid { get; }

    public IList<IStrategy> Strategies { get; }

    public IList<string> Steps { get; } = new List<string>();

    public IList<IStrategy> UsedStrategies { get; } = new List<IStrategy>();

    public Solution(Grid grid, IList<IStrategy> strategies)
    {
        Grid = grid;
        Strategies = strategies;
    }

    private void ComputeCandidates()
    {
        foreach (var cell in Grid.Where(c => !c.HasDigit)) {
            cell.Candidates.AddAll();
        }

        foreach (var constraint in Grid.Constraints) {
            constraint.Apply();
        }
    }

    public void Solve()
    {
        ComputeCandidates();
        var more = true;
        while (more) {
            more = false;
            foreach (var strategy in Strategies) {
                var outcome = strategy.Apply(Grid);
                if (outcome.HasChanges) {
                    more = Grid.CountUnplacedDigits() > 0;
                    _usedStrategies.Add(strategy);
                    Steps.Add(outcome.Description);
                    break;
                }
            }
        }

        UsedStrategies.Clear();
        foreach (var strategy in Strategies) {
            if (_usedStrategies.Contains(strategy)) {
                UsedStrategies.Add(strategy);
            }
        }
    }
}
