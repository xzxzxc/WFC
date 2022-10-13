using System.ComponentModel.DataAnnotations;
using WFC.Models;

namespace Api;

public class RootConfiguration
{
	[Required]
	public Dictionary<string, PossibleValues> PossibleValues { get; set; } = null!;

	public RandomConfiguration Random { get; set; } = new();

	public class RandomConfiguration
	{
		public int? Seed { get; set; }
	}
}

public class PossibleValues
{
	[Required]
	public string Type { get; set; } = string.Empty;

	[Required]
	public PossibleValue[] Values { get; set; } = null!;
}