namespace WFC.Exceptions;

public class CanvasIsEmptyException : Exception
{
	public CanvasIsEmptyException()
		: base("Canvas is empty")
	{
	}
}
