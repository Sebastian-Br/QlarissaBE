
namespace Qlarissa.Infrastructure;

public static class Tag
{
    public static string Security(int id) => $"s:{id}";

    public static string[] Watchlist(int id, IEnumerable<int> watchedSecurityIds) => [$"wl:{id}", .. watchedSecurityIds.Select(sid => Security(sid))];
}
