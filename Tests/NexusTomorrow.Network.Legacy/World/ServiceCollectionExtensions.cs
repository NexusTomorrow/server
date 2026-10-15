using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Network.World.Chat;
using NexusTomorrow.Network.World.Message;

namespace NexusTomorrow.Network.World;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register the world chat format and message models.
    /// </summary>
    public static void AddNetworkWorld(this IServiceCollection sc)
    {
        sc.AddNetworkWorldChat();
        sc.AddNetworkWorldMessage();
    }
}
