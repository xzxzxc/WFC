using System.Collections;
using System.Text;

namespace WFC.Models;

public record Canvas(string ValuesName, int Width, int Height) :
	IEnumerable<Cell>
{
	private readonly string?[,] _elements = new string?[Width, Height];
	
	public IEnumerable<Cell> EmptyCells => this.Where(cell => cell.IsEmpty);

	public override string ToString()
	{
		var builder = new StringBuilder();
		for (var y = 0; y < Height; y++)
		{
			for (var x = 0; x < Width; x++)
				builder.Append(this[x, y] ?? "?");

			if (y < Height - 1)
				builder.Append('\n');
		}

		return builder.ToString();
	}

	public string? this[int x, int y]
	{
		get => _elements[x, y];
		set => _elements[x, y] = value;
	}

	public IEnumerator<Cell> GetEnumerator()
	{
		for (var y = 0; y < Height; y++)
		for (var x = 0; x < Width; x++)
			yield return new Cell(x, y, this[x, y]);
	}

	IEnumerator IEnumerable.GetEnumerator() =>
		GetEnumerator();

	public void Set(Cell cell, string value) => this[cell.X, cell.Y] = value;

	public void Unset(Cell cell) => this[cell.X, cell.Y] = null;

	public bool CheckBorders(int x, int y) => 
		x >= 0 && x < Width && y >= 0 && y < Height;
}
