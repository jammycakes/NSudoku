using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
