using WFC.Internals.Sides.Abstractions;
using WFC.Models;

namespace WFC.Internals.Sides;

internal class Right : BaseSide<Right>
{
	public override Cell? GetCellOnTheSide(Canvas canvas, Cell cell)
	{
		var rightCellX = cell.X + 1;
		var rightCellY = cell.Y;
		if (!canvas.CheckBorders(rightCellX, rightCellY))
			return null;

		return new Cell(rightCellX, rightCellY, canvas[rightCellX, rightCellY]);
	}

	public override IReadOnlyCollection<string> GetSuitableValuesOnTheSide(PossibleValue possibleValue) =>
		possibleValue.SuitableValuesLeft;
}