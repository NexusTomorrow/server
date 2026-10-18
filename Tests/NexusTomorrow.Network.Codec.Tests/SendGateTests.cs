using NexusTomorrow.Network.Codec.Tests.Fixtures;

namespace NexusTomorrow.Network.Codec.Tests;

public sealed class SendGateTests
{
    [Fact]
    public void ALayoutAtItsDefaultsHasNothingUnverifiedAndIsDefault()
    {
        var holder = new GateHolder();
        Assert.Null(holder.FindUnverifiedValue());
        Assert.True(holder.IsDefault());
    }

    [Fact]
    public void VerifiedMembersMayHoldAnyValue()
    {
        var holder = new GateHolder { Known = 5, KnownSelector = 1 };
        holder.KnownInner.Known = 7;
        holder.KnownList.Add(new GateInner { Known = 9 });
        holder.KnownUnion = new GateArmOne { Known = 3 };

        Assert.Null(holder.FindUnverifiedValue());
        Assert.False(holder.IsDefault());
    }

    [Theory]
    [InlineData("Unread", "Unread")]
    [InlineData("UnreadFlag", "UnreadFlag")]
    [InlineData("UnreadFloat", "UnreadFloat")]
    [InlineData("UnreadText", "UnreadText")]
    [InlineData("UnreadList", "UnreadList")]
    [InlineData("UnreadFixed", "UnreadFixed")]
    [InlineData("UnreadInner", "UnreadInner")]
    [InlineData("KnownInner.Unread", "KnownInner.Unread")]
    [InlineData("KnownList.Unread", "KnownList[1].Unread")]
    [InlineData("KnownUnion.Unread", "KnownUnion.Unread")]
    [InlineData("UnionOfUnreadSelector.Arm", "UnreadSelector")]
    [InlineData("UnreadUnion.Arm", "UnreadUnion")]
    [InlineData("UnreadUnion.Unread", "UnreadUnion")]
    public void AnUnverifiedValueIsFoundByItsPath(string change, string expected)
    {
        GateHolder holder = Changed(change);
        Assert.Equal(expected, holder.FindUnverifiedValue());
        Assert.False(holder.IsDefault());
    }

    [Fact]
    public void AnUnverifiedValueInABaseLayoutIsFound()
    {
        Assert.Equal("UnreadInBase", new GateDerived { UnreadInBase = 1 }.FindUnverifiedValue());
        Assert.Null(new GateDerived { Known = 1 }.FindUnverifiedValue());
    }

    [Fact]
    public void TheCheckAllocatesNothingWhenItFindsNothing()
    {
        var holder = new GateHolder { Known = 5 };
        holder.KnownList.Add(new GateInner { Known = 1 });
        holder.KnownUnion = new GateArmOne { Known = 3 };
        for (int i = 0; i < 10; i++)
            _ = holder.FindUnverifiedValue();

        long before = GC.GetAllocatedBytesForCurrentThread();
        for (int i = 0; i < 1000; i++)
            _ = holder.FindUnverifiedValue();

        Assert.Equal(0L, GC.GetAllocatedBytesForCurrentThread() - before);
    }

    private static GateHolder Changed(string change)
    {
        var holder = new GateHolder();
        switch (change)
        {
            case "Unread":
                holder.Unread = 1;
                break;
            case "UnreadFlag":
                holder.UnreadFlag = true;
                break;
            case "UnreadFloat":
                holder.UnreadFloat = 0.5f;
                break;
            case "UnreadText":
                holder.UnreadText = "x";
                break;
            case "UnreadList":
                holder.UnreadList.Add(0);
                break;
            case "UnreadFixed":
                holder.UnreadFixed[2] = 1;
                break;
            case "UnreadInner":
                // An unmarked layout holds nothing at all, whatever its own members' marks.
                holder.UnreadInner.Known = 1;
                break;
            case "KnownInner.Unread":
                holder.KnownInner.Unread = 1;
                break;
            case "KnownList.Unread":
                holder.KnownList.Add(new GateInner { Known = 1 });
                holder.KnownList.Add(new GateInner { Unread = 1 });
                break;
            case "KnownUnion.Unread":
                holder.KnownUnion = new GateArmOne { Unread = 1 };
                break;
            case "UnionOfUnreadSelector.Arm":
                holder.UnionOfUnreadSelector = new GateArmOne { Known = 1 };
                break;
            case "UnreadUnion.Arm":
                holder.UnreadUnion = new GateArmOne();
                break;
            case "UnreadUnion.Unread":
                holder.UnreadUnion = new GateArmZero { Unread = 1 };
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(change), change, null);
        }

        return holder;
    }
}
