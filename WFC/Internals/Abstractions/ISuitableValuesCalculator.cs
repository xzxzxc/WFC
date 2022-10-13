using WFC.Models;

namespace WFC.Internals.Abstractions;

public interface ISuitableValuesCalculator
{
	IReadOnlyCollection<string>? Get(Canvas canvas, Cell cell);
}