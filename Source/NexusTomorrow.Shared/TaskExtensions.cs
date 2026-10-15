using System;
using System.Threading.Tasks;

namespace NexusTomorrow.Shared;

public static class TaskExtensions
{
    public static async void FireAndForgetAsync(this Task task, Action<Exception>? onException = null)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            onException?.Invoke(ex);
        }
    }
}
