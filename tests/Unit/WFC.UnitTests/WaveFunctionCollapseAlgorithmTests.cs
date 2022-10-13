using Autofac;
using Autofac.Extensions.Testing;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using WFC.Exceptions;
using WFC.Internals;
using WFC.Internals.Abstractions;
using WFC.Models;
using WFC.UnitTests.Helpers;
using Xunit;

namespace WFC.UnitTests;

public class WaveFunctionCollapseAlgorithmTests :
	TestsBase<WfcAlgorithm>
{
	private static readonly string? FirstCellValue = nameof(FirstCellValue);
	private const string TestValuesName = TestCanvasFactory.TestValuesName;

	private readonly Canvas _canvas2x1 = new(TestValuesName, Width: 2, Height: 1)
	{
		[x: 0, y: 0] = FirstCellValue,
	};

	private readonly Canvas _canvas3x1 = new(TestValuesName, Width: 3, Height: 1)
	{
		[x: 0, y: 0] = FirstCellValue,
	};
	
	private readonly Canvas _canvas4x1 = new(TestValuesName, Width: 4, Height: 1)
	{
		[x: 0, y: 0] = FirstCellValue,
	};

	protected override void ConfigureContainer(ContainerBuilder builder)
	{
		builder.RegisterType<SimpleCellSetter>().AsImplementedInterfaces();
	}

	public WaveFunctionCollapseAlgorithmTests()
		: base(isStrict: true)
	{
	}

	[Fact]
	public void ShouldThrowIfCanvasIsEmpty()
	{
		using var _ = new AssertionScope();
		var action = () => Sut.Collapse(
			TestCanvasFactory.Create(width: 1, height: 1));

		action.Should().Throw<CanvasIsEmptyException>();
	}

	[Fact]
	public void ShouldFillAllEmptyCells()
	{
		using var _ = new AssertionScope();
		var canvas = _canvas3x1;
		var secondSuitable = GetNextSuitable(canvas, index: 1, count: 1);
		var thirdSuitable = GetNextSuitable(canvas, index: 2, count: 1);
		Mock<ISuitableSelector>()
			.SetupSequence(f => f.WithMinEntropy(canvas))
			.Returns(secondSuitable)
			.Returns(thirdSuitable)
			.Throws<MethodShouldNotHaveOtherCallsException>();
		FakeRandomDoNothing();

		Sut.Collapse(canvas);

		canvas.AsEnumerable().Should().SatisfyRespectively(
			first => first.Value.Should().Be(FirstCellValue),
			second => second.Value.Should().Be(secondSuitable.SuitableValues.Single()),
			third => third.Value.Should().Be(thirdSuitable.SuitableValues.Single()));
	}

	[Fact]
	public void ShouldShuffleSuitable()
	{
		using var _ = new AssertionScope();
		var canvas = _canvas2x1;
		var secondSuitable = GetNextSuitable(canvas, index: 1, count: 2);
		Mock<ISuitableSelector>()
			.Setup(f => f.WithMinEntropy(canvas))
			.Returns(secondSuitable);
		Mock<IRandom>()
			.Setup(r => r.Shuffle(secondSuitable.SuitableValues))
			.Returns(secondSuitable.SuitableValues.Reverse().ToArray());

		Sut.Collapse(canvas);

		canvas.AsEnumerable().Should().SatisfyRespectively(
			first => first.Value.Should().Be(FirstCellValue),
			second => second.Value.Should().Be(secondSuitable.SuitableValues.Last()));
	}

	[Fact]
	public void ShouldCheckNextSuitableIfPreviousHasNoSolution()
	{
		using var _ = new AssertionScope();
		var canvas = _canvas3x1;
		var secondSuitable = GetNextSuitable(canvas, index: 1, count: 2);
		var thirdSuitable = GetNextSuitable(canvas, index: 2, count: 1);
		Mock<ISuitableSelector>()
			.SetupSequence(f => f.WithMinEntropy(canvas))
			.Returns(secondSuitable)
			.Returns(default(NextSuitable?))
			.Returns(thirdSuitable)
			.Throws<MethodShouldNotHaveOtherCallsException>();
		FakeRandomDoNothing();

		Sut.Collapse(canvas);

		canvas.AsEnumerable().Should().SatisfyRespectively(
			first => first.Value.Should().Be(FirstCellValue),
			second => second.Value.Should().Be(secondSuitable.SuitableValues.Last()),
			third => third.Value.Should().Be(thirdSuitable.SuitableValues.Single()));
	}

	[Fact]
	public void ShouldThrowIfNoSolution()
	{
		using var _ = new AssertionScope();
		var canvas = _canvas2x1;
		Mock<ISuitableSelector>()
			.SetupSequence(f => f.WithMinEntropy(canvas))
			.Returns(default(NextSuitable?))
			.Throws<MethodShouldNotHaveOtherCallsException>();
		
		var action = () => Sut.Collapse(canvas);

		action.Should().Throw<CanvasHaveNoSolution>()
			.WithMessage($"Canvas have no solution:\n{FirstCellValue}?");
	}

	[Fact]
	public void ShouldClearChangesIfNoSolution()
	{
		using var _ = new AssertionScope();
		var canvas = _canvas3x1;
		var secondSuitable = GetNextSuitable(canvas, index: 1, count: 1);
		Mock<ISuitableSelector>()
			.SetupSequence(f => f.WithMinEntropy(canvas))
			.Returns(secondSuitable)
			.Returns(default(NextSuitable?))
			.Throws<MethodShouldNotHaveOtherCallsException>();
		FakeRandomDoNothing();

		var action = () => Sut.Collapse(canvas);
		action.Should().Throw<CanvasHaveNoSolution>();
		
		canvas.AsEnumerable().Should().SatisfyRespectively(
			first => first.Value.Should().Be(FirstCellValue),
			second => second.Value.Should().BeNull(),
			third => third.Value.Should().BeNull());
	}

	[Fact]
	public void ShouldTryPreviousIfNoSolution()
	{
		using var _ = new AssertionScope();
		var canvas = _canvas4x1;
		var secondSuitable = GetNextSuitable(canvas, index: 1, count: 2);
		var thirdSuitable = GetNextSuitable(canvas, index: 2, count: 1);
		var fourthSuitable = GetNextSuitable(canvas, index: 3, count: 1);
		Mock<ISuitableSelector>()
			.SetupSequence(f => f.WithMinEntropy(canvas))
			.Returns(secondSuitable)
			.Returns(thirdSuitable)
			.Returns(default(NextSuitable?))
			.Returns(thirdSuitable)
			.Returns(fourthSuitable)
			.Throws<MethodShouldNotHaveOtherCallsException>();
		FakeRandomDoNothing();

		Sut.Collapse(canvas);

		canvas.AsEnumerable().Should().SatisfyRespectively(
			first => first.Value.Should().Be(FirstCellValue),
			second => second.Value.Should().Be(secondSuitable.SuitableValues.Last()),
			third => third.Value.Should().Be(thirdSuitable.SuitableValues.Single()),
			fourth => fourth.Value.Should().Be(fourthSuitable.SuitableValues.Single()));
	}

	private void FakeRandomDoNothing()
	{
		Mock<IRandom>()
			.Setup(r => r.Shuffle(It.IsAny<IReadOnlyCollection<string>>()))
			.Returns<IReadOnlyCollection<string>>(c => c);
	}

	private NextSuitable GetNextSuitable(
		Canvas canvas,
		Index index,
		int count) =>
		new(
			canvas.ElementAt(index),
			Faker().Make(count, () => Faker().Random.String2(1)).ToArray());
}