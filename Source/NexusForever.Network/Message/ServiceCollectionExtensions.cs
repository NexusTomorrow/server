using Microsoft.Extensions.DependencyInjection;
using NexusForever.Network.Message.Model;

namespace NexusForever.Network.Message
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register the envelope message models.
        /// </summary>
        public static void AddNetworkMessage(this IServiceCollection sc)
        {
            sc.AddNetworkMessageModel();
        }
    }
}
