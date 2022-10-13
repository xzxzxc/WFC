using WFC.Internals.Sides.Abstractions;
using WFC.Models;

namespace WFC.Internals.Sides;

internal class Left : BaseSide<Left>
{
	public override Cell? GetCellOnTheSide(Canvas canvas, Cell cell)
	{
		var leftCellX = cell.X - 1;
		var leftCellY = cell.Y;
		if (!canvas.CheckBorders(leftCellX, leftCellY))
			return null;

		return new Cell(leftCellX, leftCellY, canvas[leftCellX, leftCellY]);
	}

	public override IReadOnlyCollection<string> GetSuitableValuesOnTheSide(
		PossibleValue possibleValue) =>
		possibleValue.SuitableValuesRight;
}