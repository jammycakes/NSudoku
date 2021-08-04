using NSudoku.Constraints;

namespace NSudoku
{
    public static class GridExtensions
    {
        public static Grid AddDefaultConstraints(this Grid grid)
        {
            for (byte b = 1; b <= grid.Size; b++) {
                grid.Constraints.Add(new RowConstraint(grid, b));
                grid.Constraints.Add(new ColumnConstraint(grid, b));
                grid.Constraints.Add(new DefaultRegionConstraint(grid, b));
            }

            return grid;
        }
    }
}
