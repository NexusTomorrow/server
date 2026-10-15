using Microsoft.Extensions.DependencyInjection;
using NexusForever.Network.World.Chat;
using NexusForever.Network.World.Message;

namespace NexusForever.Network.World
{
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
}
