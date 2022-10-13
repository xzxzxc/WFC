using FluentAssertions;
using WFC.Models;

namespace WFC.AcceptanceTests;

public static class CanvasExtensions
{
	public static void Assert(this Canvas canvas, string expected)
	{
		canvas.ToString().Should().Be(expected);
	}
}
