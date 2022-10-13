using WFC.Internals.CellSetters;
using WFC.Models;

namespace WFC.Internals;

internal class SimpleCellSetter : ICellSetter
{
	public void Set(Canvas canvas, Cell cell, string value)
	{
		canvas.Set(cell, value);
	}

	public void Unset(Canvas canvas, Cell cell)
	{
		canvas.Unset(cell);
	}
}