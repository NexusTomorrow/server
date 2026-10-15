using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Network.Auth.Message;

namespace NexusTomorrow.Network.Auth;

public static class ServiceCollectionExtensions
{
    public static void AddNetworkAuth(this IServiceCollection sc)
    {
        sc.AddNetworkAuthMessage();
    }
}
