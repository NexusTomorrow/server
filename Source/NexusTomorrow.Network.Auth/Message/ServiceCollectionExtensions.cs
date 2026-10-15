using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Shared;

namespace NexusTomorrow.Network.Auth.Message
{
    public static class ServiceCollectionExtensions
    {
        public static void AddNetworkAuthMessage(this IServiceCollection sc)
        {
            TypeWalker.Walk<MessageAttribute>(Assembly.GetExecutingAssembly(), (t, a) =>
            {
                if (t.IsAssignableTo(typeof(IReadable)))
                    sc.AddKeyedTransient(typeof(IReadable), a.Opcode, t);
                if (t.IsAssignableTo(typeof(IWritable)))
                    sc.AddKeyedTransient(typeof(IWritable), a.Opcode, t);
            });
        }
    }
}
