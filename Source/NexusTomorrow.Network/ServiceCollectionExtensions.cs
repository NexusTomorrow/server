using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register the network message models.
    /// </summary>
    public static void AddNetwork(this IServiceCollection sc)
    {
        sc.AddNetworkMessage();
    }
}
