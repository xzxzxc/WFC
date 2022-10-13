using System.Collections;

namespace WFC.Models;

public class PossibleValuesCollection : IReadOnlyCollection<PossibleValue>
{
	private PossibleValue[] _values = Array.Empty<PossibleValue>();

	public void Set(IEnumerable<PossibleValue> values)
	{
		_values = values.ToArray();
	}

	public IEnumerator<PossibleValue> GetEnumerator() =>
		_values.AsEnumerable().GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

	public int Count => _values.Length;
}
