using Microsoft.Extensions.Options;
using WFC.Exceptions;
using WFC.Extensions;
using WFC.Internals.Abstractions;
using WFC.Internals.Sides;
using WFC.Internals.Sides.Abstractions;
using WFC.Models;

namespace WFC.Internals;

public class SuitableValuesCalculator : ISuitableValuesCalculator
{
	private readonly IOptionsMonitor<PossibleValuesCollection> _values;

	public SuitableValuesCalculator(
		IOptionsMonitor<PossibleValuesCollection> values)
	{
		_values = values;
	}

	public IReadOnlyCollection<string>? Get(Canvas canvas, Cell cell) =>
		GetSuitableValuesAccordingTo(Left.Instance, canvas, cell)
			.IntersectIfNotNull(GetSuitableValuesAccordingTo(Right.Instance, canvas, cell))
			.IntersectIfNotNull(GetSuitableValuesAccordingTo(Bottom.Instance, canvas, cell))
			.IntersectIfNotNull(GetSuitableValuesAccordingTo(Top.Instance, canvas, cell))
			?.ToArray();

	private IReadOnlyCollection<string>? GetSuitableValuesAccordingTo(
		ISide side,
		Canvas canvas,
		Cell cell)
	{
		var cellOnTheSide = side.GetCellOnTheSide(canvas, cell);
		return cellOnTheSide is { IsEmpty: false }
			? side.GetSuitableValuesOnTheSide(
				GetPossibleValue(canvas, cellOnTheSide.Value))
			: null;
	}

	private PossibleValue GetPossibleValue(Canvas canvas, Cell cell)
	{
		var result = _values.Get(canvas.ValuesName).SingleOrDefault(c => c.Value.Equals(cell.Value));
		return result == default
			? throw new PossibleValueIsMissingException(cell.Value)
			: result;
	}
}