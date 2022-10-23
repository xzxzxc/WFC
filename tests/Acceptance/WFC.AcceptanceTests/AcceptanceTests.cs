using AcceptanceTests.Common;
using Autofac;
using Autofac.Extensions;
using WFC.DI;
using WFC.Models;
using Xunit;
using Xunit.Abstractions;

namespace WFC.AcceptanceTests;

public class AcceptanceTests
{
	private readonly ITestOutputHelper _testOutputHelper;
	private const string TestValuesName = nameof(TestValuesName);

	// ┠ ┨ ┰ ┸

	private readonly IWfcAlgorithm _wfc;

	public AcceptanceTests(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
		var containerBuilder = new ContainerBuilder();
		containerBuilder.RegisterModule(new WfcModule(randomSeed: 69));
		containerBuilder.Populate(
			collection => collection.AddPossibleValues(
				TestValuesName,
				TestPossibleValuesCollection.Unicode));

		_wfc = containerBuilder.Build()
			.Resolve<IWfcAlgorithm>();
	}

	[Fact]
	public void ShouldCollapseSimple()
	{
		var canvas = new Canvas(TestValuesName, Width: 2, Height: 1)
		{
			[x: 0, y: 0] = "┰"
		};

		_wfc.Collapse(canvas);

		canvas.Assert("┰┨");
	}

	[Fact]
	public void ShouldCollapseComplicated()
	{
		var canvas = new Canvas(TestValuesName, Width: 10, Height: 10)
		{
			[x: 3, y: 3] = "┰",
			[x: 3, y: 4] = "┸",
			[x: 4, y: 3] = "┸",
			[x: 4, y: 4] = "┰",
		};

		_wfc.Collapse(canvas);

		canvas.Assert(
			@"┨┠┸┸┨┠┸┨ ┠
┸┨  ┠┸┰┸┰┨
┰┨  ┠┰┸┰┨┠
┨┠┰┰┸┨ ┠┨┠
┨┠┸┸┰┸┰┨┠┨
┸┨  ┠┰┨┠┨┠
 ┠┰┰┨┠┸┸┸┨
 ┠┸┨┠┸┰┰┰┸
 ┠┰┨┠┰┨┠┨ 
 ┠┨┠┸┨┠┨┠┰");
	}

	[Fact]
	public void Generate()
	{
		var canvas = new Canvas(TestValuesName, Width: 25, Height: 50)
		{
			[x: 3, y: 3] = "┰",
			[x: 3, y: 4] = "┠",
			[x: 4, y: 3] = "┸",
			[x: 4, y: 4] = "┰",
		};

		try
		{
			_wfc.Collapse(canvas);
		}
		finally
		{
			_testOutputHelper.WriteLine($"Canvas:\n{canvas}");
		}
	}
}
