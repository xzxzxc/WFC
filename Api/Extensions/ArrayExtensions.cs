namespace Api.Extensions;

public static class ArrayExtensions
{
	public static string?[] ToOneDimensionalArray(this string?[,] strings)
	{
		var width = strings.GetLength(0);
		var height = strings.GetLength(1);
		var res = new string?[width * height];
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
				res[x + y * width] = strings[x, y];
		}

		return res;
	}
}
