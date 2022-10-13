namespace WFC.Extensions;

public static class ArrayExtensions
{
	public static IEnumerable<T> ToEnumerable<T>(this T[,] array)
	{
		for (var i = 0; i < array.GetLength(dimension: 0); i++)
		for (var j = 0; j < array.GetLength(dimension: 1); j++)
			yield return array[i, j];
	}
}
