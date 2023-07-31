using System.Collections.Generic;
using System.Linq;

namespace NSudoku.Constraints;

public class DefaultRegionConstraint : CellConstraint
{
    private readonly byte _region;

    public DefaultRegionConstraint(Grid grid, byte region)
        : base(GetCells(grid, region), true)
    {
        _region = region;
    }

    private static readonly int[] regionWidths = new[] {
        0, 1, 2, 3,
        2, 5, 3, 7,
        4, 3, 5, 11,
        4, 13, 7, 5,
        4, 17, 6, 19,
        5, 7, 11, 23,
        6, 5, 13, 9,
        7, 29, 6, 31,
        8
    };

    private static IEnumerable<Cell> GetCells(Grid grid, byte region)
    {
        var regionWidth = regionWidths[grid.Size];
        var regionHeight = grid.Size / regionWidth;

        var left = ((region - 1) % regionWidth) * regionWidth + 1;
        var top = ((region - 1) / regionHeight) * regionHeight + 1;

        return
            from row in Enumerable.Range(top, regionHeight)
            from column in Enumerable.Range(left, regionWidth)
            select grid[(byte)row, (byte)column];
    }

    public override string ToString() => $"box {_region}";
}
