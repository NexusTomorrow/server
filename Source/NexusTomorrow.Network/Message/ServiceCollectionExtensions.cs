using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Network.Message.Model;

namespace NexusTomorrow.Network.Message
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
