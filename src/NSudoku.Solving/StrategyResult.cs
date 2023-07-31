using System;
using System.Collections.Generic;
using System.Linq;

namespace NSudoku.Solving;

public class StrategyResult
{
    private StrategyResult()
    {
        HasChanges = false;
        Description = String.Empty;
        CellsChanged = new List<Cell>();
    }

    public StrategyResult(string description, params Cell[] cellsChanged)
    {
        HasChanges = true;
        Description = description;
        CellsChanged = cellsChanged;
    }

    public StrategyResult(string description, IEnumerable<Cell> cellsChanged)
    {
        HasChanges = true;
        Description = description;
        CellsChanged = cellsChanged.ToList();
    }

    public ICollection<Cell> CellsChanged { get; }

    public bool HasChanges { get; }

    public string Description { get; }

    public static StrategyResult Unchanged { get; } = new StrategyResult();
}
