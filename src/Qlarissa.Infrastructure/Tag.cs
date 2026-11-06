
namespace Qlarissa.Infrastructure;

public static class Tag
{
    public static string Security(int id) => $"security:{id}";

    public static string[] Watchlist(int id, IEnumerable<int> watchedSecurityIds) => [$"watchlist:{id}", .. watchedSecurityIds.Select(sid => Security(sid))];
}
