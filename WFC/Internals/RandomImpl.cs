using MoreLinq.Extensions;
using WFC.Internals.Abstractions;

namespace WFC.Internals;

internal class RandomImpl : IRandom
{
	private readonly Random _random;

	public RandomImpl(int? seed)
	{
		_random = seed.HasValue
			? new Random(seed.Value)
			: new Random();
	}

	public IReadOnlyCollection<T> Shuffle<T>(IReadOnlyCollection<T> elements) =>
		elements.Shuffle(_random).ToArray();
}
