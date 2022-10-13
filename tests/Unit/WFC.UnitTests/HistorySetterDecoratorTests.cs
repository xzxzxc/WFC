using Autofac;
using Autofac.Extensions.Testing;
using FluentAssertions;
using Moq;
using WFC.Extensions;
using WFC.Internals;
using WFC.Internals.CellSetters;
using WFC.Models;
using WFC.UnitTests.Helpers;
using Xunit;

namespace WFC.UnitTests;

public class HistorySetterDecoratorTests :
	TestsBase<SaveHistoryDecorator>
{
	protected override void ConfigureContainer(ContainerBuilder builder)
	{
		builder.RegisterType<SimpleCellSetter>().AsImplementedInterfaces();
	}

	[Fact]
	public void ShouldSaveHistory()
	{
		var canvas = TestCanvasFactory.Create(width: 2, height: 1);

		Sut.Set(canvas, new Cell(X: 0, Y: 0), value: "1");
		Sut.Set(canvas, new Cell(X: 1, Y: 0), value: "2");

		Sut.History.Should().SatisfyRespectively(
			first => first.ToEnumerable().Should().BeEquivalentTo("1", null),
			second => second.ToEnumerable().Should().BeEquivalentTo("1", "2"));
	}
}