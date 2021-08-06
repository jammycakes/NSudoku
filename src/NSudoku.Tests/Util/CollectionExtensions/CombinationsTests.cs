using System.Linq;
using FluentAssertions;
using NSudoku.Util;

using Xunit;

namespace NSudoku.Tests.Util.CollectionExtensions
{
    public class CombinationsTests
    {
        [Fact]
        public void ShouldSelectThreeOfFive()
        {
            var numbers = new[] {1, 2, 3, 4, 5};

            var combinations = numbers.Combinations(3).ToList();
            var results = new[] {
                new[] {1, 2, 3},
                new[] {1, 2, 4},
                new[] {1, 2, 5},
                new[] {1, 3, 4},
                new[] {1, 3, 5},
                new[] {1, 4, 5},
                new[] {2, 3, 4},
                new[] {2, 3, 5},
                new[] {2, 4, 5},
                new[] {3, 4, 5}
            };

            for (var i = 0; i < combinations.Count; i++) {
                combinations[i].Should().BeEquivalentTo(results[i]);
            }
        }

        [Fact]
        public void ShouldSelectTwoOfTwo()
        {
            var numbers = new[] {1, 2};

            var combinations = numbers.Combinations(2).ToList();
            var results = new[] {
                new[] {1, 2}
            };

            for (var i = 0; i < combinations.Count; i++) {
                combinations[i].Should().BeEquivalentTo(results[i]);
            }
        }


        [Fact]
        public void ShouldSelectTwoOfThree()
        {
            var numbers = new[] {1, 2, 3};

            var combinations = numbers.Combinations(2).ToList();
            var results = new[] {
                new[] {1, 2},
                new[] {1, 3},
                new[] {2, 3},
            };

            for (var i = 0; i < combinations.Count; i++) {
                combinations[i].Should().BeEquivalentTo(results[i]);
            }
        }

        [Fact]
        public void ShouldSelectZeroOfThree()
        {
            var numbers = new[] {1, 2, 3};
            var combinations = numbers.Combinations(0).ToList();
            combinations.Should().BeEmpty();
        }

        [Fact]
        public void ShouldSelectZeroOfZero()
        {
            var numbers = new[] {1, 2, 3};
            var combinations = numbers.Combinations(0).ToList();
            combinations.Should().BeEmpty();
        }


        [Fact]
        public void ShouldSelectThreeOfTwo()
        {
            var numbers = new[] {1, 2};
            var combinations = numbers.Combinations(3).ToList();
            combinations.Should().BeEmpty();
        }
    }
}
