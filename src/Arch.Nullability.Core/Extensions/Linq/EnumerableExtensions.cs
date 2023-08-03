namespace Arch.Nullability.Core.Extensions.Linq;

public static class EnumerableExtensions
{
	public static IEnumerable<TItem> Tap<TItem>(this IEnumerable<TItem> items, Action<TItem> action)
	{
		foreach (var item in items)
		{
			action(item);
			yield return item;
		}
	}
}
