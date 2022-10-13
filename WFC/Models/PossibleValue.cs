namespace WFC.Models;

public readonly record struct PossibleValue(
	string Value,
	IReadOnlyCollection<string> SuitableValuesTop,
	IReadOnlyCollection<string> SuitableValuesRight,
	IReadOnlyCollection<string> SuitableValuesBottom,
	IReadOnlyCollection<string> SuitableValuesLeft);