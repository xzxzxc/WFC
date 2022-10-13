using WFC.Models;

namespace WFC.Internals.CellSetters;

public abstract class BaseCellSetterDecorator : ICellSetter
{
	private readonly ICellSetter _cellSetter;

	public BaseCellSetterDecorator(ICellSetter cellSetter)
	{
		_cellSetter = cellSetter;
	}

	public virtual void Set(Canvas canvas, Cell cell, string value)
	{
		_cellSetter.Set(canvas, cell, value);
	}

	public virtual void Unset(Canvas canvas, Cell cell)
	{
		_cellSetter.Unset(canvas, cell);
	}
}
