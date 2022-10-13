using WFC.Models;

namespace WFC.Internals.Sides.Abstractions;

internal interface ISide
{
	Cell? GetCellOnTheSide(Canvas canvas, Cell cell);

	IReadOnlyCollection<string> GetSuitableValuesOnTheSide(PossibleValue possibleValue);
}