using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NSudoku.Constraints;

namespace NSudoku
{
    public class Grid : IEnumerable<Cell>
    {
        public const byte MaxSize = 32;

        private Cell[][] _cells;

        public byte Size { get; }

        public IList<IConstraint> Constraints { get; } = new List<IConstraint>();

        public Grid(byte size)
        {
            if (size < 4 || size > MaxSize) {
                throw new ArgumentException(
                    $"Grid size must be between 4 and {MaxSize}");
            }

            this.Size = size;
            this._cells =
                Enumerable.Range(1, size)
                    .Select(row =>
                        Enumerable.Range(1, size)
                            .Select(column => new Cell(this, new CellRef((byte)row, (byte)column, this)))
                            .ToArray()
                    ).ToArray();
        }

        public int CountUnplacedDigits() => this.Where(c => !c.HasDigit).Count();

        public int CountCandidates() => this.Where(c => !c.HasDigit).Sum(c => c.Candidates.Count());

        public Cell this[CellRef index] => _cells[index.Row - 1][index.Column - 1];

        public Cell this[byte row, byte column] => _cells[row - 1][column - 1];

        /// <summary>
        ///  Gets the cells that are visible to the given cell. This does not include the cell itself.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public HashSet<Cell> GetCellsVisibleTo(Cell cell)
        {
            var cells = new HashSet<Cell>();
            foreach (var constraint in this.Constraints) {
                cells.UnionWith(constraint.GetCellsVisibleTo(cell));
            }

            return cells;
        }

        /// <summary>
        ///  Gets the cells that are visible to all of the given cells.
        ///  This does not include the given cells.
        /// </summary>
        /// <param name="cellRefs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IEnumerable<Cell> GetCellsVisibleToAll(params Cell[] cells)
        {
            if (cells.Length == 0) {
                return Enumerable.Empty<Cell>();
            }

            var seen = GetCellsVisibleTo(cells[0]);
            foreach (var cell in cells.Skip(1)) {
                seen.IntersectWith(GetCellsVisibleTo(cell));
            }

            seen.ExceptWith(cells);
            return seen;
        }

        /// <summary>
        ///  Gets the cells that are visible to any of the given cells.
        ///  This does not include the given cells.
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        public IEnumerable<Cell> GetCellsVisibleToAny(params Cell[] cells)
        {
            return cells
                .SelectMany(GetCellsVisibleTo)
                .Distinct()
                .Except(cells);
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            foreach (var row in _cells) {
                foreach (var cell in row) {
                    yield return cell;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Concat(this.Select(c => c.Digit ?? (byte)0));
        }

        public static Grid Parse(string givens)
        {
            var size = (int)Math.Floor(Math.Sqrt(givens.Length));
            if (size * size != givens.Length) {
                throw new ArgumentException("Length of the givens string is not a perfect square.");
            }

            var grid = new Grid((byte)size).AddDefaultConstraints();
            for (var ix = 0; ix < givens.Length; ix++) {
                var row = (byte)(ix / size + 1);
                var column = (byte)(ix % size + 1);
                if (byte.TryParse(givens.Substring(ix, 1), out var digit) && digit > 0) {
                    grid[row, column].SetGivenDigit(digit);
                }
            }

            return grid;
        }
    }
}
