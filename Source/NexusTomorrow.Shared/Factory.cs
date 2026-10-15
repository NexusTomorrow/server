using System;
using Microsoft.Extensions.DependencyInjection;

namespace NexusTomorrow.Shared;

public class Factory<T> : IFactory<T> where T : class
{
    private readonly IServiceProvider _serviceProvider;

    public Factory(
        IServiceProvider sp)
    {
        _serviceProvider = sp;
    }

    public T Resolve()
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}
