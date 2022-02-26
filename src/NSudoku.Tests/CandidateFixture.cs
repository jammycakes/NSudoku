using FluentAssertions;
using Xunit;

namespace NSudoku.Tests;

public class CandidateFixture
{
    [Fact]
    public void SetCandidateShouldWork()
    {
        var candidates = new Candidates(9);
        candidates.Has(1).Should().BeFalse();
        candidates.Add(1).Should().BeTrue();
        candidates.Has(1).Should().BeTrue();
    }

    [Fact]
    public void RemoveCandidateShouldWork()
    {
        var candidates = new Candidates(9);
        candidates.AddAll();
        candidates.Has(5).Should().BeTrue();
        candidates.Remove(5).Should().BeTrue();
        candidates.Has(5).Should().BeFalse();
    }

    [Fact]
    public void AddAllShouldAddAll()
    {
        var candidates = new Candidates(9);
        candidates.AddAll();
        for (byte i = 1; i <= 9; i++) {
            candidates.Has(i).Should().BeTrue();
        }
    }

    [Fact]
    public void ClearShouldClear()
    {
        var candidates = new Candidates(9);
        candidates.AddAll();
        candidates.Clear();
        for (byte i = 1; i <= 9; i++) {
            candidates.Has(i).Should().BeFalse();
        }
    }

    [Fact]
    public void CountCorrectlyWhenAllAdded()
    {
        var candidates = new Candidates(9);
        candidates.AddAll();
        candidates.Count.Should().Be(9);
    }

    [Fact]
    public void CountCorrectlyWhenIndividualsAdded()
    {
        var candidates = new Candidates(9);
        candidates.Add(1);
        candidates.Add(3);
        candidates.Add(7);
        candidates.Count.Should().Be(3);
    }
}
