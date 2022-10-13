using WFC.Models;

namespace WFC.Internals.Abstractions;

public interface ISuitableSelector
{
	NextSuitable? WithMinEntropy(Canvas canvas);
}