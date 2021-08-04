using System;
using System.Linq;

namespace NSudoku
{
    public class Grid
    {
        private Cell[][] _cells;

        public int Size { get; }

        public Grid(int size)
        {
            if (size < 4 || size > CellRef.MaxSize) {
                throw new ArgumentException(
                    $"Grid size must be between 4 and {CellRef.MaxSize}");
            }

            this.Size = size;
            this._cells =
                Enumerable.Range(1, size)
                    .Select(row =>
                        Enumerable.Range(1, size)
                            .Select(column => new Cell(this, new CellRef(row, column)))
                            .ToArray()
                    ).ToArray();
        }

        public Cell this[CellRef index] => _cells[index.Row - 1][index.Column - 1];
    }
}
