using WFC.Internals.Abstractions;
using WFC.Models;

namespace WFC.Internals;

public class SuitableSelector : ISuitableSelector
{
	private readonly ISuitableValuesCalculator _suitableValuesCalculator;

	public SuitableSelector(ISuitableValuesCalculator suitableValuesCalculator)
	{
		_suitableValuesCalculator = suitableValuesCalculator;
	}

	public NextSuitable? WithMinEntropy(Canvas canvas)
	{
		var suitableValueses = canvas.EmptyCells
			.Select(
				cell => (
					cell,
					suitableValues: _suitableValuesCalculator.Get(canvas, cell)))
			.Where(				c => c.suitableValues != null)
			.ToArray();

		if (!suitableValueses.Any())
			return null;
		
		var (cell, suitableValues) = suitableValueses
			.MinBy(c => c.suitableValues!.Count);

		return new NextSuitable(cell, suitableValues!);
	}
}
