using WFC.Exceptions;
using WFC.Extensions;
using WFC.Internals.Abstractions;
using WFC.Internals.CellSetters;
using WFC.Models;

namespace WFC.Internals;

public class WfcAlgorithm : IWfcAlgorithm
{
	private readonly ISuitableSelector _suitableSelector;
	private readonly IRandom _random;
	private readonly ICellSetter _cellSetter;

	public WfcAlgorithm(
		ISuitableSelector suitableSelector,
		IRandom random,
		ICellSetter cellSetter)
	{
		_suitableSelector = suitableSelector;
		_random = random;
		_cellSetter = cellSetter;
	}

	public void Collapse(Canvas canvas)
	{
		if (canvas.All(c => c.IsEmpty))
			throw new CanvasIsEmptyException();

		if (!CollapseRecursive(canvas)) 
			throw new CanvasHaveNoSolution(canvas);
	}

	private bool CollapseRecursive(Canvas canvas)
	{
		if (canvas.EmptyCells.IsEmpty())
			return true;

		var nextSuitable = _suitableSelector.WithMinEntropy(canvas);
		if (!nextSuitable.HasValue)
			return false;
		var (cell, suitableValues) = nextSuitable.Value;
		var suitableValuesQueue = new Queue<string>(_random.Shuffle(suitableValues));

		while (true)
		{
			if (!suitableValuesQueue.TryDequeue(out var value))
			{
				_cellSetter.Unset(canvas, cell);
				return false;
			}

			_cellSetter.Set(canvas, cell, value);
			if (!CollapseRecursive(canvas))
				continue;

			return true;
		}
	}
}