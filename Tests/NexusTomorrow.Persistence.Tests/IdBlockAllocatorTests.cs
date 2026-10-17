using NexusTomorrow.Persistence.Realm;

namespace NexusTomorrow.Persistence.Tests;

public sealed class IdBlockAllocatorTests
{
    [Fact]
    public void IdsComeFromTheBlocksInOrder()
    {
        var allocator = new IdBlockAllocator(refillBelow: 2);
        allocator.Add(new IdBlock(1, 3));
        allocator.Add(new IdBlock(1001, 2));
        var ids = new List<ulong>();
        while (allocator.TryTake(out ulong id))
            ids.Add(id);
        Assert.Equal([1UL, 2, 3, 1001, 1002], ids);
    }

    [Fact]
    public void ARefillIsWantedOnceBelowTheMarkAndUntilItArrives()
    {
        var allocator = new IdBlockAllocator(refillBelow: 2);
        allocator.Add(new IdBlock(1, 3));
        Assert.False(allocator.WantsRefill);
        allocator.TryTake(out _);
        allocator.TryTake(out _);
        Assert.True(allocator.WantsRefill);
        allocator.MarkRefillRequested();
        Assert.False(allocator.WantsRefill);
        allocator.Add(new IdBlock(1001, 1000));
        Assert.False(allocator.WantsRefill);
    }
}
