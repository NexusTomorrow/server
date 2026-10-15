using System;
using Microsoft.Extensions.DependencyInjection;

namespace NexusTomorrow.Shared;

public class FactoryInterface<T> : IFactoryInterface<T> where T : class
{
    private readonly IServiceProvider _serviceProvider;

    public FactoryInterface(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T2 Resolve<T2>() where T2 : T
    {
        return _serviceProvider.GetRequiredService<T2>();
    }
}
