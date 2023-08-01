using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NSudoku;

public class Candidates : IEnumerable<byte>
{
    private byte _size;

    private uint _selection;

    public Candidates(byte size)
    {
        _size = size;
    }

    private uint GetMask(byte digit)
    {
        if (digit < 1 || digit > _size) {
            throw new ArgumentOutOfRangeException($"Digits must be between 1 and {_size}");
        }

        return 1u << (digit - 1);
    }

    public bool Has(byte digit)
    {
        return (_selection & GetMask(digit)) != 0;
    }

    public bool Add(byte digit)
    {
        var previous = _selection;
        _selection |= GetMask(digit);
        return _selection != previous;
    }

    public bool Add(params byte[] digits)
    {
        var result = false;
        foreach (var digit in digits) {
            result = Add(digit) || result;
        }

        return result;
    }

    public bool Remove(byte digit)
    {
        var previous = _selection;
        _selection &= ~ GetMask(digit);
        return _selection != previous;
    }

    public bool Remove(params byte[] digits)
    {
        var result = false;
        foreach (var digit in digits) {
            result = Remove(digit) || result;
        }

        return result;
    }

    public void AddAll()
    {
        _selection = (1u << _size) - 1;
    }

    public void Clear()
    {
        _selection = 0;
    }

    public int Count
    {
        get {
            var i = _selection;
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (int)(((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }
    }

    public IEnumerator<byte> GetEnumerator()
    {
        for (byte i = 1; i <= _size; i++) {
            if (Has(i)) {
                yield return i;
            }
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is Candidates other) {
            return other._selection == this._selection;
        }
        else {
            return false;
        }
    }

    public override int GetHashCode()
    {
        unchecked {
            return (int)_selection;
        }
    }

    public override string ToString()
        => String.Concat(this.Select(b => b.ToString()));

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Candidates Copy(Candidates other)
    {
        _selection = other._selection;
        return this;
    }
}
