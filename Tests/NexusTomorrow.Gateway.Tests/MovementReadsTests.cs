using System.Numerics;
using System.Reflection;
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
    // The arms the relay carries: the client's built set (Movement-0018), less the forced-move arms 3 and 25.
    private static readonly int[] s_relayArms = [0, 1, 2, 8, 10, 11, 13, 14, 18, 20, 24, 27];

    [Fact]
    public void EveryMemberTheMovementReadIsVerified()
    {
        foreach ((Type type, string member) in MovementReads.Members)
        {
            PropertyInfo? property = type.GetProperty(member);
            Assert.True(property is not null, $"{type.Name}.{member} does not exist");
            Assert.True(property.IsDefined(typeof(VerifiedMemberAttribute), inherit: false), $"{type.Name}.{member} is not marked verified");
        }
    }

    [Fact]
    public void EveryMemberOfEveryRelayedArmIsInTheReadList()
    {
        Type[] arms = [.. typeof(IServer0262Union2).Assembly.GetTypes()
            .Where(type => typeof(IServer0262Union2).IsAssignableFrom(type) && type.GetCustomAttribute<UnionCaseAttribute>() is { } arm
                && s_relayArms.Contains((int)arm.Value))];
        Assert.Equal(s_relayArms.Length, arms.Length);

        foreach (Type arm in arms)
        {
            foreach (PropertyInfo property in arm.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                Assert.True(MovementReads.Members.Contains((arm, property.Name)), $"{arm.Name}.{property.Name} is not in the read list");
        }

        Assert.Contains((typeof(ClientMovement), nameof(ClientMovement.ClientTime)), MovementReads.Members);
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
    public void TheMovementsRowLimitsItsBodyTo2048Bytes()
    {
        Assert.Equal(2048, OpcodeTable.All[0x0637].MaxBodyBytes);
        Assert.All(OpcodeTable.All.Values.Where(row => row.Opcode != 0x0637), row => Assert.Null(row.MaxBodyBytes));
    }

    [Fact]
    public void ABatchOfConfirmedArmsKeepsItsOrderAndTime()
    {
        ClientMovement message = Movement(Time(5250), Position(1, 2, 3), Facing(0.5f));
        message.ClientTime = 5000;
        bool onPlatform = false;

        MovementRead read = MovementReads.Read(message, ref onPlatform);

        Assert.Equal(MovementRefusal.None, read.Refusal);
        RelayBatch batch = Assert.IsType<RelayBatch>(read.Batch);
        Assert.Equal(5000u, batch.Time);
        byte[] arms = [0, 2, 14];
        Assert.Equal(arms, batch.Commands.Select(command => command.Unknown0));
        Assert.Same(message.Commands[1], batch.Commands[1]);
    }

    [Fact]
    public void ArmsThreeAndTwentyFiveAreNamedAndLeftOut()
    {
        ClientMovement message = Movement(Time(1), Keyed(3), Position(1, 2, 3), Keyed(25));
        bool onPlatform = false;

        MovementRead read = MovementReads.Read(message, ref onPlatform);

        Assert.Equal(ForcedMoveArms.Arm3 | ForcedMoveArms.Arm25, read.Forced);
        byte[] arms = [0, 2];
        Assert.Equal(arms, read.Batch!.Commands.Select(command => command.Unknown0));
    }

    [Fact]
    public void AnArmOutsideTheConfirmedListRefusesTheMessage()
    {
        ClientMovement message = Movement(Time(1), new Server0262Part5 { Unknown0 = 9, Unknown1 = new Server0262Union2Arm09() }, Position(1, 2, 3));
        bool onPlatform = false;

        MovementRead read = MovementReads.Read(message, ref onPlatform);

        Assert.Equal(MovementRefusal.UnconfirmedArm, read.Refusal);
        Assert.Null(read.Batch);
        Assert.False(read.Routes);
    }

    [Fact]
    public void ARefusedMessageLeavesThePlatformStateAlone()
    {
        ClientMovement message = Movement(Platform(77), new Server0262Part5 { Unknown0 = 26, Unknown1 = new Server0262Union2Arm1A() });
        bool onPlatform = false;

        MovementReads.Read(message, ref onPlatform);

        Assert.False(onPlatform);
    }

    [Fact]
    public void AMessageOfOnlyForcedMoveArmsPostsNoBatch()
    {
        bool onPlatform = false;

        MovementRead read = MovementReads.Read(Movement(Keyed(3), Keyed(25)), ref onPlatform);

        Assert.Null(read.Batch);
        Assert.Null(read.Sample.Position);
        Assert.Null(read.Sample.Facing);
        Assert.False(read.Routes);
    }

    [Fact]
    public void ACommandWithANonFiniteValueIsLeftOutOfTheBatch()
    {
        bool onPlatform = false;

        MovementRead read = MovementReads.Read(Movement(Position(1, 2, 3), Position(4, 5, float.NaN), Facing(float.PositiveInfinity)), ref onPlatform);

        RelayBatch batch = Assert.IsType<RelayBatch>(read.Batch);
        Assert.Single(batch.Commands);
        Assert.Equal(new Vector3(1, 2, 3), Assert.IsType<SetPositionCommand>(batch.Commands[0].Unknown1).Position);
    }

    [Fact]
    public void TheLastPositionAndFacingAreTaken()
    {
        ClientMovement message = Movement(Position(1, 2, 3), Facing(0.25f), Position(4, 5, 6));
        bool onPlatform = false;

        MovementRead read = MovementReads.Read(message, ref onPlatform);

        Assert.True(read.Routes);
        Assert.Equal(new Vector3(4, 5, 6), read.Sample.Position);
        Assert.Equal(0.25f, read.Sample.Facing);
    }

    [Fact]
    public void AMessageOfTimesAloneGivesABatchAndNoSample()
    {
        bool onPlatform = false;

        MovementRead read = MovementReads.Read(Movement(Time(1000)), ref onPlatform);

        Assert.Null(read.Sample.Position);
        Assert.Null(read.Sample.Facing);
        Assert.Single(read.Batch!.Commands);
        Assert.True(read.Routes);
    }

    [Fact]
    public void APositionOnAPlatformIsNotTakenUntilThePlatformIsZeroAgain()
    {
        bool onPlatform = false;

        Assert.Null(MovementReads.Read(Movement(Platform(77), Position(1, 1, 1)), ref onPlatform).Sample.Position);
        Assert.True(onPlatform);

        Assert.Null(MovementReads.Read(Movement(Position(2, 2, 2)), ref onPlatform).Sample.Position);

        Assert.Equal(new Vector3(9, 9, 9), MovementReads.Read(Movement(Platform(0), Position(9, 9, 9)), ref onPlatform).Sample.Position);
    }

    [Fact]
    public void ANaNPositionAfterAFiniteOneLeavesTheFiniteOne()
    {
        bool onPlatform = false;

        MovementRead read = MovementReads.Read(Movement(Position(1, 2, 3), Position(4, 5, float.NaN)), ref onPlatform);

        Assert.Equal(new Vector3(1, 2, 3), read.Sample.Position);
    }

    [Fact]
    public void AnInfiniteFacingAloneGivesNoFacing()
    {
        bool onPlatform = false;

        MovementRead read = MovementReads.Read(Movement(Position(1, 2, 3), Facing(float.PositiveInfinity)), ref onPlatform);

        Assert.Equal(new Vector3(1, 2, 3), read.Sample.Position);
        Assert.Null(read.Sample.Facing);
    }

    [Fact]
    public void AMessageWhoseOnlyPositionIsNotFiniteAndWithNoFacingRoutesNothing()
    {
        bool onPlatform = false;

        MovementRead read = MovementReads.Read(Movement(Position(1, float.NegativeInfinity, 3)), ref onPlatform);

        Assert.Null(read.Sample.Position);
        Assert.Null(read.Batch);
        Assert.False(read.Routes);
    }

    private static ClientMovement Movement(params Server0262Part5[] commands)
    {
        var message = new ClientMovement();
        message.Commands.AddRange(commands);
        return message;
    }

    private static Server0262Part5 Time(uint time) => new() { Unknown0 = 0, Unknown1 = new SetTimeCommand { Time = time } };

    private static Server0262Part5 Keyed(byte arm) =>
        arm == 3 ? new() { Unknown0 = 3, Unknown1 = new Server0262Union2Arm03() } : new() { Unknown0 = 25, Unknown1 = new Server0262Union2Arm19() };

    private static Server0262Part5 Platform(uint unit) => new() { Unknown0 = 1, Unknown1 = new SetPlatformCommand { PlatformUnitId = unit } };

    private static Server0262Part5 Position(float x, float y, float z) =>
        new() { Unknown0 = 2, Unknown1 = new SetPositionCommand { Position = new Vector3(x, y, z) } };

    private static Server0262Part5 Facing(float yaw) =>
        new() { Unknown0 = 14, Unknown1 = new SetRotationCommand { Rotation = new Vector3(yaw, 0, 0) } };
}
