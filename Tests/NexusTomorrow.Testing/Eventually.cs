namespace NexusTomorrow.Testing;

public static class Eventually
{
    public static async Task TrueAsync(Func<bool> condition, TimeSpan? timeout = null)
    {
        DateTime deadline = DateTime.UtcNow + (timeout ?? TimeSpan.FromSeconds(10));
        while (!condition())
        {
            if (DateTime.UtcNow > deadline)
                throw new TimeoutException("The condition never held.");
            await Task.Delay(10);
        }
    }
}
