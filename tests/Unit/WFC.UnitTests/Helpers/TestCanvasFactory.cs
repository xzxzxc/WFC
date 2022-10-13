using WFC.Models;

namespace WFC.UnitTests.Helpers;

public static class TestCanvasFactory
{
	public const string TestValuesName = nameof(TestValuesName);
	
	public static Canvas Create(int width, int height) => 
		new(TestValuesName, width, height);
}
