namespace Api.Models.Results;

public record CollapseWithHistoryResult(
	int Width, 
	int Height,
	IReadOnlyCollection<IReadOnlyCollection<string?>> ValuesHistory);
