using WFC.Models;

namespace WFC.Internals.CellSetters;

public interface ICellSetter
{
	void Set(Canvas canvas, Cell cell, string value);

	void Unset(Canvas canvas, Cell cell);
}
