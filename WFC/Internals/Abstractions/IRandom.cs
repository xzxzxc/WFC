namespace WFC.Internals.Abstractions;

public interface IRandom
{
	IReadOnlyCollection<T> Shuffle<T>(
		IReadOnlyCollection<T> elements);
}