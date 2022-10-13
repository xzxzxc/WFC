using WFC.Internals.Sides.Abstractions;
using WFC.Models;

namespace WFC.Internals.Sides;

internal class Top : BaseSide<Top>
{
	public override Cell? GetCellOnTheSide(Canvas canvas, Cell cell)
	{
		var topCellX = cell.X;
		var topCellY = cell.Y - 1;
		if (!canvas.CheckBorders(topCellX, topCellY))
			return null;

		return new Cell(topCellX, topCellY, canvas[topCellX, topCellY]);
	}

	public override IReadOnlyCollection<string> GetSuitableValuesOnTheSide(PossibleValue possibleValue) =>
		possibleValue.SuitableValuesBottom;
}