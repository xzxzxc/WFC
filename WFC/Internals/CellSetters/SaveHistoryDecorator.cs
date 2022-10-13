using WFC.Exceptions;
using WFC.Models;

namespace WFC.Internals.CellSetters;

public class SaveHistoryDecorator : 
	BaseCellSetterDecorator,
	IHistoryHolder
{
	private readonly List<string?[,]> _items = new();

	public SaveHistoryDecorator(
		ICellSetter cellSetter)
		: base(cellSetter)
	{
	}

	public IReadOnlyCollection<string?[,]> History => _items;

	public override void Set(Canvas canvas, Cell cell, string value)
	{
		base.Set(canvas, cell, value);
		AddToHistory(canvas);
	}

	public override void Unset(Canvas canvas, Cell cell)
	{
		base.Unset(canvas, cell);
		AddToHistory(canvas);
	}

	private void AddToHistory(Canvas canvas)
	{
		if (_items.Count > 1_000)
			throw new HistoryIsTooLongException();

		var elements = GetElements(canvas);
		_items.Add(elements);
	}

	private static string?[,] GetElements(Canvas canvas)
	{
		var elements = new string?[canvas.Width, canvas.Height];
		for (var x = 0; x < canvas.Width; x++)
		{
			for (var y = 0; y < canvas.Height; y++)
				elements[x, y] = canvas[x, y];
		}

		return elements;
	}
}