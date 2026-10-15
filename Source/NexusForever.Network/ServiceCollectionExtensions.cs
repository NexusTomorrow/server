using Microsoft.Extensions.DependencyInjection;
using NexusForever.Network.Message;

namespace NexusForever.Network
{
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
}
