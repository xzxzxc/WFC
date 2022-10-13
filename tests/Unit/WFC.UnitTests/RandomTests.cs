using Autofac.Extensions.Testing;
using FluentAssertions;
using WFC.Extensions;
using WFC.Internals;
using Xunit;

namespace WFC.UnitTests;

public class RandomTests : TestsBase
{
	private RandomImpl Sut { get; } = new(seed: 69);

	[Fact]
	public void ShouldPickRandomly()
	{
		var oneElementCount = 10_000;
		var allowedDeviation = 0.05;
		var elems = Enumerable.Range(start: 0, count: 10).ToArray();
		var distribution = new int[elems.Length, elems.Length];

		foreach (var _ in Enumerable.Range(start: 0, oneElementCount * elems.Length))
		{
			var newElems = (int[])Sut.Shuffle(elems);
			for (var i = 0; i < newElems.Length; i++)
				distribution[i, newElems[i]]++;
		}

		distribution.ToEnumerable().Should()
			.AllSatisfy(e => e.Should().BeCloseTo(
				oneElementCount,
				(uint)(oneElementCount * allowedDeviation)));
	}
}