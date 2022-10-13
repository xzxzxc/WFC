using Autofac.Extensions.Testing;
using FluentAssertions;
using Moq;
using WFC.Internals;
using WFC.Internals.Abstractions;
using WFC.Models;
using WFC.UnitTests.Helpers;
using Xunit;

namespace WFC.UnitTests;

public class NextSuitableFinderTests : TestsBase<SuitableSelector>
{
	private const string TestValuesName = TestCanvasFactory.TestValuesName;
	private readonly Canvas _canvas = new(TestValuesName, Width: 3, Height: 1)
	{
		[x: 0, y: 0] = "test value",
	};

	[Fact]
	public void ShouldReturnNullIfAllSuitableIsNotDefinedForAllCells()
	{
		FakeSuitableValuesCalculator(x: 1, collection: null);

		var actual = Sut.WithMinEntropy(_canvas);

		actual.Should().BeNull();
	}

	[Fact]
	public void ShouldIgnoreCellsIfSuitableIsNotDefined()
	{
		FakeSuitableValuesCalculator(x: 1, collection: null);
		FakeSuitableValuesCalculator(x: 2, count: 1);
		
		var actual = Sut.WithMinEntropy(_canvas);

		actual.Should().NotBeNull();
		actual!.Value.Cell.X.Should().Be(2);
	}

	[Fact]
	public void ShouldReturnWithSmallestEntropyFirst()
	{
		FakeSuitableValuesCalculator(x: 1, count: 4);
		FakeSuitableValuesCalculator(x: 2, count: 3);
		
		var actual = Sut.WithMinEntropy(_canvas);

		actual.Should().NotBeNull();
		actual!.Value.Cell.X.Should().Be(2);
	}

	private void FakeSuitableValuesCalculator(int x, int count) =>
		FakeSuitableValuesCalculator(x, Enumerable.Range(1, count).Select(i => i.ToString()).ToArray());
	
	private void FakeSuitableValuesCalculator(
		int x, 
		IReadOnlyCollection<string>? collection) =>
		Mock<ISuitableValuesCalculator>()
			.Setup(
				calc => calc.Get(
					_canvas,
					It.Is<Cell>(cell => cell.Y == 0 && cell.X == x)))
			.Returns(collection);
}
