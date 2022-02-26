using System;

namespace NSudoku;

public struct CellRef
{
    public byte Row { get; }

    public byte Column { get; }

    internal CellRef(byte row, byte column, Grid grid)
    {
        if (row < 1 || row > grid.Size || column < 1 || column > grid.Size) {
            throw new ArgumentException($"Cell references must be between 1 and {grid.Size}");
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
        return ((int)Row) * Grid.MaxSize + Column;
    }

    public static bool operator ==(CellRef ref1, CellRef ref2) => ref1.Equals(ref2);

    public static bool operator !=(CellRef ref1, CellRef ref2) => !ref1.Equals(ref2);

    public override string ToString()
    {
        return $"r{Row}c{Column}";
    }
}
