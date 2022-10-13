namespace WFC.Models;

public readonly record struct Cell(int X, int Y, string? Value = default)
{
	public bool IsEmpty => Value is null;
}
