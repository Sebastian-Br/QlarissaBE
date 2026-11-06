
namespace Qlarissa.Infrastructure;

public static class Key
{
    public static string FullSecurityKey(int id) => $"s:f:{id}";
    public static string BasicSecurityKey(int id) => $"s:b:{id}";
}
