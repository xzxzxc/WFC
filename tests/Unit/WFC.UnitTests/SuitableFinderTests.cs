using Autofac;
using Autofac.Extensions.Testing;
using FluentAssertions;
using WFC.Exceptions;
using WFC.Internals;
using WFC.Models;
using WFC.UnitTests.Helpers;
using Xunit;

namespace WFC.UnitTests;

public class SuitableFinderTests : TestsBase<SuitableValuesCalculator>
{
	private const string TestValuesName = TestCanvasFactory.TestValuesName;
	private static readonly PossibleValue PossibleValue =
		new(
			Value: "1",
			SuitableValuesTop: new[]
			{
				"2", "3", "4",
			},
			SuitableValuesRight: new[]
			{
				"2", "5", "6"
			},
			SuitableValuesBottom: new[]
			{
				"3", "5", "7"
			},
			SuitableValuesLeft: new[]
			{
				"4", "6", "7"
			});

	protected override void ConfigureContainer(ContainerBuilder builder)
	{
		var possibleValuesCollection = new PossibleValuesCollection();
		possibleValuesCollection.Set(new[]
		{
			PossibleValue,
		});
		builder.RegisterOptionsStub(possibleValuesCollection);
	}

	[Fact]
	public void ShouldThrowForMissingPossibleValue()
	{
		var canvas = new Canvas(TestValuesName, Width: 2, Height: 1)
		{
			[x: 0, y: 0] = "test value"
		};
		
		var get = () => Sut.Get(canvas, new Cell(X: 0, Y: 1));

		get.Should().Throw<PossibleValueIsMissingException>();
	}

	[Fact]
	public void ShouldReturnNullIfNoNeighboursFilled()
	{
		var canvas = new Canvas(TestValuesName, Width: 3, Height: 1)
		{
			[x: 0, y: 0] = "test value"
		};
		
		var actual = Sut.Get(canvas, new Cell(X: 0, Y: 2));

		actual.Should().BeNull();
	}

	[Theory]
	[MemberData(nameof(ShouldHandleBordersCases))]
	public void ShouldHandleBorders(Canvas canvas, int x, int y)
	{
		var action = () => Sut.Get(canvas, new Cell(X: x, Y: y));

		action.Should().NotThrow();
	}

	[Fact]
	public void ShouldHandleValueOnTheLeft()
	{
		var canvas2X1 = CreateCanvas2x1();
		SetValue(canvas2X1, x: 0, y: 0);

		var res = Sut.Get(canvas2X1, new Cell(X: 1, Y: 0));

		res.Should().BeEquivalentTo(PossibleValue.SuitableValuesRight);
	}

	[Fact]
	public void ShouldHandleValueOnTheRight()
	{
		var canvas2X1 = CreateCanvas2x1();
		SetValue(canvas2X1, x: 1, y: 0);

		var res = Sut.Get(canvas2X1, new Cell(X: 0, Y: 0));

		res.Should().BeEquivalentTo(PossibleValue.SuitableValuesLeft);
	}

	[Fact]
	public void ShouldHandleValueOnTheBottom()
	{
		var canvas1X2 = CreateCanvas1x2();
		SetValue(canvas1X2, x: 0, y: 1);

		var res = Sut.Get(canvas1X2, new Cell(X: 0, Y: 0));

		res.Should().BeEquivalentTo(PossibleValue.SuitableValuesTop);
	}

	[Fact]
	public void ShouldHandleValueOnTheTop()
	{
		var canvas1X2 = CreateCanvas1x2();
		SetValue(canvas1X2, x: 0, y: 0);

		var res = Sut.Get(canvas1X2, new Cell(X: 0, Y: 1));

		res.Should().BeEquivalentTo(PossibleValue.SuitableValuesBottom);
	}

	[Fact]
	public void ShouldHandleValueOnTheLeftAndBottom()
	{
		var canvas2X2 = CreateCanvas2x2();
		SetValue(canvas2X2, x: 0, y: 0);
		SetValue(canvas2X2, x: 1, y: 1);

		var res = Sut.Get(canvas2X2, new Cell(X: 1, Y: 0));

		res.Should().BeEquivalentTo(
			PossibleValue.SuitableValuesRight
				.Intersect(PossibleValue.SuitableValuesTop));
	}

	[Fact]
	public void ShouldHandleValueOnTheRightAndTop()
	{
		var canvas2X2 = CreateCanvas2x2();
		SetValue(canvas2X2, x: 1, y: 1);
		SetValue(canvas2X2, x: 0, y: 0);

		var res = Sut.Get(canvas2X2, new Cell(X: 0, Y: 1));

		res.Should().BeEquivalentTo(
			PossibleValue.SuitableValuesLeft
				.Intersect(PossibleValue.SuitableValuesBottom));
	}

	[Fact]
	public void ShouldHandleValueOnTheLeftAndTop()
	{
		var canvas2X2 = CreateCanvas2x2();
		SetValue(canvas2X2, x: 0, y: 0);
		SetValue(canvas2X2, x: 1, y: 1);

		var res = Sut.Get(canvas2X2, new Cell(X: 0, Y: 1));

		res.Should().BeEquivalentTo(
			PossibleValue.SuitableValuesLeft
				.Intersect(PossibleValue.SuitableValuesBottom));
	}

	[Fact]
	public void ShouldHandleValueOnTheRightAndBottom()
	{
		var canvas2X2 = CreateCanvas2x2();
		SetValue(canvas2X2, x: 1, y: 0);
		SetValue(canvas2X2, x: 0, y: 1);

		var res = Sut.Get(canvas2X2, new Cell(X: 0, Y: 0));

		res.Should().BeEquivalentTo(
			PossibleValue.SuitableValuesLeft
				.Intersect(PossibleValue.SuitableValuesTop));
	}

	private static void SetValue(Canvas canvas, int x, int y) => 
		canvas[x, y] = PossibleValue.Value;

	public static IEnumerable<object[]> ShouldHandleBordersCases()
	{
		var canvas = CreateCanvas2x1();
		SetValue(canvas, x: 1, y: 0);
		yield return new object[]
		{
			canvas, 0, 0
		};
		canvas = CreateCanvas2x1();
		SetValue(canvas, x: 0, y: 0);
		yield return new object[]
		{
			canvas, 1, 0
		};
		canvas = CreateCanvas1x2();
		SetValue(canvas, x: 0, y: 1);
		yield return new object[]
		{
			canvas, 0, 0
		};
		canvas = CreateCanvas1x2();
		SetValue(canvas, x: 0, y: 0);
		yield return new object[]
		{
			canvas, 0, 1
		};
		canvas = CreateCanvas2x2();
		SetValue(canvas, x: 0, y: 0);
		yield return new object[]
		{
			canvas, 0, 1
		};
		yield return new object[]
		{
			canvas, 1, 0
		};
		yield return new object[]
		{
			canvas, 1, 1
		};
	}

	private static Canvas CreateCanvas2x1() => 
		TestCanvasFactory.Create(width: 2, height: 1);

	private static Canvas CreateCanvas1x2() => 
		TestCanvasFactory.Create(width: 1, height: 2);

	private static Canvas CreateCanvas2x2() => 
		TestCanvasFactory.Create(width: 2, height: 2);
	
	private static Canvas CreateCanvas3x2() => 
		TestCanvasFactory.Create(width: 2, height: 2);
}
