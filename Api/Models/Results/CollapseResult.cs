namespace Api.Models.Results;

public record CollapseResult(
	int Width,
	int Height,
	IReadOnlyCollection<string> Values);