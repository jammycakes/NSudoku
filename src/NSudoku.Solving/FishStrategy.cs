using System.Collections.Generic;
using System.Linq;
using NSudoku.Constraints;
using NSudoku.Util;

namespace NSudoku.Solving
{
    /// <summary>
    ///  Implements the "fish" class of sudoku solving techniques (X-wing, swordfish, jellyfish etc).
    /// </summary>
    public class FishStrategy : IStrategy
    {
        private readonly int _size;
        private readonly string _name;

        public FishStrategy(int size, string name)
        {
            _size = size;
            _name = name;
        }

        public StrategyResult Apply(Grid grid)
        {
            var rows = grid.Constraints.OfType<RowConstraint>().ToArray();
            var columns = grid.Constraints.OfType<ColumnConstraint>().ToArray();

            var result = GoFishing(grid, rows, columns);
            if (!result.HasChanges) {
                result = GoFishing(grid, columns, rows);
            }

            return result;
        }

        private StrategyResult GoFishing(Grid grid, ICellConstraint[] dimension1, ICellConstraint[] dimension2)
        {
            var combinations = dimension1.Combinations(_size);
            foreach (var combination in combinations) {
                for (byte candidate = 1; candidate <= grid.Size; candidate++) {
                    var cells =
                        from line in combination
                        from cell in line.Cells
                        where !cell.HasDigit
                              && cell.Candidates.Has(candidate)
                        select cell;
                    var rows = cells.Select(c => c.Ref.Row).ToHashSet();
                    var columns = cells.Select(c => c.Ref.Column).ToHashSet();
                    if (rows.Count() == _size && columns.Count() == _size) {
                        /*
                         * We have a fish in dimension 1. So eliminate the candidate from all cells
                         * in dimension 2 that are either in the same row or the same column, but
                         * not in both.
                         */
                        var eliminations =
                            from constraint in dimension2
                            from cell in constraint.Cells
                            where (rows.Contains(cell.Ref.Row) ^ columns.Contains(cell.Ref.Column))
                                  && !cell.HasDigit
                            select cell;

                        var removed = new List<Cell>();
                        foreach (var elimination in eliminations) {
                            if (elimination.Candidates.Remove(candidate)) {
                                removed.Add(elimination);
                            }
                        }

                        var strDimensions = string.Join(", ", dimension1.Select(d => d.ToString()));
                        var strRemoved = string.Join(",", removed.Select(r => r.Ref.ToString()));
                        var description =
                            $"{_name} in {strDimensions}. " +
                            $"Value {candidate} removed from {strRemoved}.";
                        return new StrategyResult(description, removed);
                    }
                }
            }

            return StrategyResult.Unchanged;
        }

        public override string ToString() => _name;

        public static FishStrategy XWing { get; } = new FishStrategy(2, "X-wing");

        public static FishStrategy Swordfish { get; } = new FishStrategy(3, "Swordfish");

        public static FishStrategy Jellyfish { get; } = new FishStrategy(4, "Jellyfish");

        public static FishStrategy Squirmbag { get; } = new FishStrategy(5, "Squirmbag");

        public static FishStrategy Whale { get; } = new FishStrategy(6, "Whale");

        public static FishStrategy Leviathan { get; } = new FishStrategy(7, "Leviathan");
    }
}
