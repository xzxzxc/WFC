using Microsoft.Extensions.DependencyInjection;
using WFC.Models;

namespace WFC.DI;

public static class ServiceCollectionExtensions
{
	public static void AddPossibleValues(
		this IServiceCollection serviceCollection,
		string valuesName,
		IEnumerable<PossibleValue> possibleValues)
	{
		serviceCollection.AddOptions<PossibleValuesCollection>(valuesName)
			.Configure(c => c.Set(possibleValues));
	}
}
