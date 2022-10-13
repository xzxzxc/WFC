namespace WFC.Exceptions;

public class PossibleValueIsMissingException : Exception
{
	public PossibleValueIsMissingException(object? valueValue)
		: base($"Possible value is missing for '{valueValue}'")
	{
	}
}
