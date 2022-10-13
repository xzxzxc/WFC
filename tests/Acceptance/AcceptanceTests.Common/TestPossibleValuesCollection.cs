using WFC.Models;

namespace AcceptanceTests.Common;

public static class TestPossibleValuesCollection
{
	public static readonly PossibleValue[] Unicode =
	{
		new(
			Value: "┠",
			SuitableValuesTop: new[]
			{
				"┨", "┰", "┠",
			},
			SuitableValuesRight: new[]
			{
				"┨", "┰", "┸",
			},
			SuitableValuesBottom: new[]
			{
				"┨", "┸", "┠",
			},
			SuitableValuesLeft: new[]
			{
				"┨", " ",
			}),
		new(
			Value: "┰",
			SuitableValuesTop: new[]
			{
				" ", "┸",
			},
			SuitableValuesRight: new[]
			{
				"┨", "┸", "┰",
			},
			SuitableValuesBottom: new[]
			{
				"┨", "┠", "┸",
			},
			SuitableValuesLeft: new[]
			{
				"┠", "┸", "┰",
			}),
		new(
			Value: "┨",
			SuitableValuesTop: new[]
			{
				"┠", "┰", "┨",
			},
			SuitableValuesRight: new[]
			{
				" ", "┠",
			},
			SuitableValuesBottom: new[]
			{
				"┠", "┸", "┨",
			},
			SuitableValuesLeft: new[]
			{
				"┠", "┰", "┸",
			}),
		new(
			Value: "┸",
			SuitableValuesTop: new[]
			{
				"┠", "┨", "┰",
			},
			SuitableValuesRight: new[]
			{
				"┨", "┰", "┸",
			},
			SuitableValuesBottom: new[]
			{
				" ", "┰",
			},
			SuitableValuesLeft: new[]
			{
				"┠", "┰", "┸",
			}),
		new(
			Value: " ",
			SuitableValuesTop: new[]
			{
				"┸", " ",
			},
			SuitableValuesRight: new[]
			{
				"┠", " ",
			},
			SuitableValuesBottom: new[]
			{
				"┰", " ",
			},
			SuitableValuesLeft: new[]
			{
				"┨", " ",
			}),
	};
}
