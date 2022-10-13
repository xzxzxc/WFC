namespace WFC;

public interface IHistoryHolder
{
	IReadOnlyCollection<string?[,]> History { get; }
}
