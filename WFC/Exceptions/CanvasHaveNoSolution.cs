using WFC.Models;

namespace WFC.Exceptions;

internal class CanvasHaveNoSolution : Exception
{
	public CanvasHaveNoSolution(Canvas canvas)
		: base($"Canvas have no solution:\n{canvas}")
	{
	}
}
