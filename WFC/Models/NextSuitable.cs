namespace WFC.Models;

public readonly record struct NextSuitable(
	Cell Cell,
	IReadOnlyCollection<string> SuitableValues);
