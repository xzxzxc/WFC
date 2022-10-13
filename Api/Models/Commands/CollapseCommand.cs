using WFC.Models;

namespace Api.Models.Commands;

public record CollapseCommand(
	string Name,
	int Width,
	int Height,
	IReadOnlyCollection<CollapseCommand.Element> Elements)
{
	public record Element(
		int X,
		int Y,
		string Value);
	
	public Canvas ToCanvas()
	{
		var cells = new Canvas(Name, Width, Height);
		foreach (var element in Elements)
			cells[element.X, element.Y] = element.Value;
		return cells;
	}
}
