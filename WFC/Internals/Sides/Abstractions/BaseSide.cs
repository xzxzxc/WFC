using WFC.Models;

namespace WFC.Internals.Sides.Abstractions;

internal abstract class BaseSide<TChild> : ISide
	where TChild : BaseSide<TChild>, new()
{
	public static TChild Instance { get; } = new();

	public abstract Cell? GetCellOnTheSide(Canvas canvas, Cell cell);

	public abstract IReadOnlyCollection<string> GetSuitableValuesOnTheSide(
		PossibleValue possibleValue);
}