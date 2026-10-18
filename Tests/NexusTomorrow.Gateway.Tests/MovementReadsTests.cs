using System.Numerics;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Gateway.World;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Owners.World;

namespace NexusTomorrow.Gateway.Tests;

public sealed class MovementReadsTests
{
    [Fact]
    public void EveryMemberTheMovementReadIsVerified()
    {
        foreach ((Type type, string member) in MovementReads.Members)
        {
            System.Reflection.PropertyInfo? property = type.GetProperty(member);
            Assert.True(property is not null, $"{type.Name}.{member} does not exist");
            Assert.True(property.IsDefined(typeof(VerifiedMemberAttribute), inherit: false), $"{type.Name}.{member} is not marked verified");
        }
    }

    [Fact]
    public void TheMovementsRowAdmitsItAtPurposeAtHighPriority()
    {
        OpcodeRow row = OpcodeTable.All[0x0637];

        Assert.True(row.ReadsVerifiedMembers);
        Assert.Equal(MessagePriority.High, row.Priority);
        Assert.Equal(OpcodeTable.WorldTag, row.Tag);
        Assert.Equal(Rung.Purpose, MessageCatalog.Default.RungOf(typeof(ClientMovement)));
    }

    [Fact]
    public void TheLastPositionAndFacingAreTaken()
    {
        ClientMovement message = Movement(Position(1, 2, 3), Facing(0.25f), Position(4, 5, 6));
        bool onPlatform = false;

        Assert.True(MovementReads.TryLatest(message, ref onPlatform, out MovementSample sample));
        Assert.Equal(new Vector3(4, 5, 6), sample.Position);
        Assert.Equal(0.25f, sample.Facing);
    }

    [Fact]
    public void AMessageOfTimesAloneGivesNothing()
    {
        ClientMovement message = Movement(new Server0262Part5 { Unknown0 = 0, Unknown1 = new Server0262Union2Arm00 { Unknown0 = 1000 } });
        bool onPlatform = false;

        Assert.False(MovementReads.TryLatest(message, ref onPlatform, out _));
    }

    [Fact]
    public void APositionOnAPlatformIsNotTakenUntilThePlatformIsZeroAgain()
    {
        bool onPlatform = false;

        Assert.False(MovementReads.TryLatest(Movement(Platform(77), Position(1, 1, 1)), ref onPlatform, out _));
        Assert.True(onPlatform);

        Assert.False(MovementReads.TryLatest(Movement(Position(2, 2, 2)), ref onPlatform, out _));

        Assert.True(MovementReads.TryLatest(Movement(Platform(0), Position(9, 9, 9)), ref onPlatform, out MovementSample sample));
        Assert.Equal(new Vector3(9, 9, 9), sample.Position);
    }

    [Fact]
    public void ANaNPositionAfterAFiniteOneLeavesTheFiniteOne()
    {
        ClientMovement message = Movement(Position(1, 2, 3), Position(4, 5, float.NaN));
        bool onPlatform = false;

        Assert.True(MovementReads.TryLatest(message, ref onPlatform, out MovementSample sample));
        Assert.Equal(new Vector3(1, 2, 3), sample.Position);
    }

    [Fact]
    public void AnInfiniteFacingAloneGivesNoFacing()
    {
        ClientMovement message = Movement(Position(1, 2, 3), Facing(float.PositiveInfinity));
        bool onPlatform = false;

        Assert.True(MovementReads.TryLatest(message, ref onPlatform, out MovementSample sample));
        Assert.Equal(new Vector3(1, 2, 3), sample.Position);
        Assert.Null(sample.Facing);
    }

    [Fact]
    public void AMessageWhoseOnlyPositionIsNotFiniteAndWithNoFacingGivesNothing()
    {
        ClientMovement message = Movement(Position(1, float.NegativeInfinity, 3));
        bool onPlatform = false;

        Assert.False(MovementReads.TryLatest(message, ref onPlatform, out _));
    }

    private static ClientMovement Movement(params Server0262Part5[] commands)
    {
        var message = new ClientMovement();
        message.Unknown1.AddRange(commands);
        return message;
    }

    private static Server0262Part5 Platform(uint unit) => new() { Unknown0 = 1, Unknown1 = new Server0262Union2Arm01 { Unknown0 = unit } };

    private static Server0262Part5 Position(float x, float y, float z) =>
        new() { Unknown0 = 2, Unknown1 = new Server0262Union2Arm02 { Unknown0 = new Vector3(x, y, z) } };

    private static Server0262Part5 Facing(float yaw) =>
        new() { Unknown0 = 14, Unknown1 = new Server0262Union2Arm0E { Unknown0 = new Vector3(yaw, 0, 0) } };
}
