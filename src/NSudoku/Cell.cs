using System;
using System.Collections;
using System.Collections.Generic;

namespace NSudoku
{
    public class Cell
    {
        private byte? _placedDigit;
        private byte? _givenDigit;

        public Cell(Grid grid, CellRef @ref)
        {
            Grid = grid;
            Ref = @ref;
            Candidates = new Candidates(Grid.Size);
        }

        public Grid Grid { get; }

        public CellRef Ref { get; }

        public Candidates Candidates { get; }

        public byte? Digit {
            get => _placedDigit ?? _givenDigit;
        }

        public bool IsGiven => _givenDigit.HasValue;

        public bool HasDigit => _givenDigit.HasValue || _placedDigit.HasValue;

        private byte? AssertRange(byte? value)
        {
            if (value.HasValue && (value < 1 || value > Grid.Size)) {
                throw new ArgumentException($"Digits must be between 1 and {Grid.Size}");
            }

            return value;
        }

        public bool PlaceDigit(byte value)
        {
            var previous = _placedDigit;
            _placedDigit = AssertRange(value);
            _givenDigit = null;
            Candidates.Clear();
            return _placedDigit != previous;
        }

        public bool SetGivenDigit(byte value)
        {
            var previous = _givenDigit;
            _givenDigit = AssertRange(value);
            _placedDigit = null;
            Candidates.Clear();
            return _givenDigit != previous;
        }
    }
}
