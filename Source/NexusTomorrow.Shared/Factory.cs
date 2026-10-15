using System;
using Microsoft.Extensions.DependencyInjection;

namespace NexusTomorrow.Shared;

public class Factory<T> : IFactory<T> where T : class
{
    private readonly IServiceProvider sp;

    public Factory(
        IServiceProvider sp)
    {
        this.sp = sp;
    }

    public T Resolve()
    {
        return sp.GetRequiredService<T>();
    }
}
