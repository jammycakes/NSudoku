using System;
using System.Numerics;

namespace NSudoku
{
    public struct CellRef
    {
        public const int MaxSize = 32;

        public int Row { get; }

        public int Column { get; }

        public CellRef(int row, int column)
        {
            if (row < 1 || row > MaxSize || column < 1 || column > MaxSize) {
                throw new ArgumentException($"Cell references must be between 1 and {MaxSize}");
            }

            Row = row;
            Column = column;
        }

        public override bool Equals(object obj)
        {
            if (obj is CellRef other) {
                return other.Row == this.Row && other.Column == this.Column;
            }
            else {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return ((int)Row) * MaxSize + Column;
        }

        public override string ToString()
        {
            return $"r{Row}c{Column}";
        }
    }
}
