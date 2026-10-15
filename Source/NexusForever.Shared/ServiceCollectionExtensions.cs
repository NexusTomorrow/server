using Microsoft.Extensions.DependencyInjection;

namespace NexusForever.Shared
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register <typeparamref name="TImplementation"/> as a transient <typeparamref name="TInterface"/> with a singleton <see cref="IFactory{T}"/> that creates it.
        /// </summary>
        public static void AddTransientFactory<TInterface, TImplementation>(this IServiceCollection sc)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            sc.AddTransient<TInterface, TImplementation>();
            sc.AddSingleton<IFactory<TInterface>, Factory<TInterface>>();
        }
    }
}
