using System;
using System.Collections;
using System.Collections.Generic;

namespace NSudoku
{
    public class Cell
    {
        private BitArray _candidates = new BitArray(CellRef.MaxSize);
        private int? _placedDigit;
        private int? _givenDigit;

        public Cell(Grid grid, CellRef @ref)
        {
            Grid = grid;
            Ref = @ref;
            _candidates = new BitArray(Grid.Size);
        }

        public Grid Grid { get; }

        public CellRef Ref { get; }

        public int? Digit {
            get => _placedDigit ?? _givenDigit;
        }

        public bool IsGiven => _givenDigit.HasValue;

        private int? AssertRange(int? value)
        {
            if (value.HasValue && (value < 1 || value > Grid.Size)) {
                throw new ArgumentException($"Digits must be between 1 and {Grid.Size}");
            }

            return value;
        }

        public int? PlacedDigit {
            get => _placedDigit;
            set {
                _placedDigit = AssertRange(value);
                _givenDigit = 0;
            }
        }

        public int? GivenDigit {
            get => _givenDigit;
            set {
                _givenDigit = AssertRange(value);
                _placedDigit = 0;
            }
        }

        public void AddCandidate(byte value)
        {
            AssertRange(value);
            _candidates[value - 1] = true;
        }

        public void RemoveCandidate(byte value)
        {
            AssertRange(value);
            _candidates[value - 1] = false;
        }

        public IEnumerable<int> GetCandidates()
        {
            for (int ix = 0; ix < Grid.Size; ix++) {
                if (_candidates[ix]) yield return ix + 1;
            }
        }

        public int NumberOfCandidates {
            get {
                Int32[] ints = new Int32[(_candidates.Count >> 5) + 1];

                _candidates.CopyTo(ints, 0);

                Int32 count = 0;

                // fix for not truncated bits in last integer that may have been set to true with SetAll()
                ints[ints.Length - 1] &= ~(-1 << (_candidates.Count % 32));

                for (Int32 i = 0; i < ints.Length; i++) {
                    Int32 c = ints[i];

                    // magic (http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel)
                    unchecked {
                        c = c - ((c >> 1) & 0x55555555);
                        c = (c & 0x33333333) + ((c >> 2) & 0x33333333);
                        c = ((c + (c >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
                    }

                    count += c;

                }

                return count;
            }
        }

        public bool IsNakedSingle =>
            !Digit.HasValue &&
            NumberOfCandidates == 1;
    }
}
