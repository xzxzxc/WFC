namespace WFC.Extensions;

internal static class EnumerableExtensions
{
	public static IEnumerable<T>? IntersectIfNotNull<T>(
		this IEnumerable<T>? source,
		IEnumerable<T>? other)
	{
		if (source is null)
			return other;
		if (other is null)
			return source;

		return source.Intersect(other);
	}
	
	public static bool IsEmpty<T>(
		this IEnumerable<T> source) =>
		!source.Any();
}