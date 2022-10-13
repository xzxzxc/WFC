using WFC.Internals.Sides.Abstractions;
using WFC.Models;

namespace WFC.Internals.Sides;

internal class Bottom : BaseSide<Bottom>
{
	public override Cell? GetCellOnTheSide(Canvas canvas, Cell cell)
	{
		var bottomCellX = cell.X;
		var bottomCellY = cell.Y + 1;
		if (!canvas.CheckBorders(bottomCellX, bottomCellY))
			return null;

		return new Cell(bottomCellX, bottomCellY, canvas[bottomCellX, bottomCellY]);
	}

	public override IReadOnlyCollection<string> GetSuitableValuesOnTheSide(PossibleValue possibleValue) =>
		possibleValue.SuitableValuesTop;
}