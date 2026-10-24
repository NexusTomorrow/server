using System.Collections.Immutable;
using System.Numerics;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Owners.World;

namespace NexusTomorrow.Gateway.World;

/// <summary>
/// Why <see cref="MovementReads.Read"/> refused a <see cref="ClientMovement"/>.
/// </summary>
public enum MovementRefusal
{
    /// <summary>
    /// Not refused.
    /// </summary>
    None,

    /// <summary>
    /// A command arm the client never builds (outside Movement-0018's list and the two forced-move arms).
    /// </summary>
    UnconfirmedArm,
}

/// <summary>
/// The forced-move command arms a <see cref="ClientMovement"/> carried. They are logged and left out of the relay, not refused, until the
/// client's keyed path that adds them is read (the design checkpoint's item 11).
/// </summary>
[Flags]
public enum ForcedMoveArms
{
    /// <summary>
    /// Neither.
    /// </summary>
    None = 0,

    /// <summary>
    /// Command 3, keyed positions.
    /// </summary>
    Arm3 = 1,

    /// <summary>
    /// Command 25, keyed states.
    /// </summary>
    Arm25 = 2,
}

/// <summary>
/// What <see cref="MovementReads.Read"/> made of a <see cref="ClientMovement"/>: the newest position and facing for the map to keep, the
/// commands to relay to the players who see the mover, why the message was refused, and the forced-move arms it carried.
/// </summary>
public readonly record struct MovementRead(MovementSample Sample, RelayBatch? Batch, MovementRefusal Refusal, ForcedMoveArms Forced)
{
    /// <summary>
    /// True when the message gives the map something: a position, a facing or commands to relay.
    /// </summary>
    public bool Routes => Refusal == MovementRefusal.None && (Batch is not null || Sample.Position is not null || Sample.Facing is not null);
}

/// <summary>
/// What the server reads from a <see cref="ClientMovement"/>, read to <c>Purpose</c>: the members <see cref="Members"/> names, each
/// marked verified, and nothing else. The opcode table admits the message for that reason.
/// </summary>
public static class MovementReads
{
    private const byte KeyedPositions = 3;
    private const byte KeyedStates = 25;

    // The arms the client builds (Movement-0018) and the relay carries; 3 and 25 come from the client's forced moves.
    private static readonly bool[] s_relayed = Arms(0, 1, 2, 8, 10, 11, 13, 14, 18, 20, 24, 27);

    /// <summary>
    /// Every member <see cref="Read"/> reads, which is every member of every arm it relays.
    /// </summary>
    public static ImmutableArray<(Type Type, string Member)> Members { get; } =
    [
        (typeof(ClientMovement), nameof(ClientMovement.ClientTime)),
        (typeof(ClientMovement), nameof(ClientMovement.Commands)),
        (typeof(Server0262Part5), nameof(Server0262Part5.Unknown0)),
        (typeof(Server0262Part5), nameof(Server0262Part5.Unknown1)),
        (typeof(SetTimeCommand), nameof(SetTimeCommand.Time)),
        (typeof(SetPlatformCommand), nameof(SetPlatformCommand.PlatformUnitId)),
        (typeof(SetPositionCommand), nameof(SetPositionCommand.Position)),
        (typeof(SetPositionCommand), nameof(SetPositionCommand.Blend)),
        (typeof(SetVelocityCommand), nameof(SetVelocityCommand.Velocity)),
        (typeof(SetVelocityCommand), nameof(SetVelocityCommand.Blend)),
        (typeof(Server0262Union2Arm0A), nameof(Server0262Union2Arm0A.Blend)),
        (typeof(SetMoveCommand), nameof(SetMoveCommand.CommandedVelocity)),
        (typeof(SetMoveCommand), nameof(SetMoveCommand.Blend)),
        (typeof(Server0262Union2Arm0D), nameof(Server0262Union2Arm0D.Blend)),
        (typeof(SetRotationCommand), nameof(SetRotationCommand.Rotation)),
        (typeof(SetRotationCommand), nameof(SetRotationCommand.Blend)),
        (typeof(Server0262Union2Arm12), nameof(Server0262Union2Arm12.UnitId)),
        (typeof(Server0262Union2Arm12), nameof(Server0262Union2Arm12.Blend)),
        (typeof(Server0262Union2Arm14), nameof(Server0262Union2Arm14.Rotation)),
        (typeof(Server0262Union2Arm14), nameof(Server0262Union2Arm14.Duration)),
        (typeof(Server0262Union2Arm14), nameof(Server0262Union2Arm14.SpinRate)),
        (typeof(Server0262Union2Arm14), nameof(Server0262Union2Arm14.Offset)),
        (typeof(Server0262Union2Arm14), nameof(Server0262Union2Arm14.Blend)),
        (typeof(SetStateCommand), nameof(SetStateCommand.State)),
        (typeof(SetModeCommand), nameof(SetModeCommand.Mode)),
    ];

    /// <summary>
    /// Read <paramref name="message"/>. A command arm the client never builds refuses the whole message, and leaves
    /// <paramref name="onPlatform"/> as it was. Otherwise:
    /// <list type="bullet">
    /// <item>The batch holds <see cref="ClientMovement.ClientTime"/> and every relayed command in the order sent, less any carrying a
    /// value that is not finite; a message with none has no batch.</item>
    /// <item>The forced-move arms 3 and 25 are named in <see cref="MovementRead.Forced"/> and left out of the batch.</item>
    /// <item>The sample is the newest finite position and facing. A platform command sets <paramref name="onPlatform"/> to whether its
    /// unit id is not 0; while it is set, positions and facings are relative to that unit and are not taken. The state carries across
    /// messages, because the client sends a platform command only when it changes.</item>
    /// </list>
    /// </summary>
    public static MovementRead Read(ClientMovement message, ref bool onPlatform)
    {
        ForcedMoveArms forced = ForcedMoveArms.None;
        int relayable = 0;
        foreach (Server0262Part5 command in message.Commands)
        {
            byte arm = command.Unknown0;
            if (arm == KeyedPositions)
                forced |= ForcedMoveArms.Arm3;
            else if (arm == KeyedStates)
                forced |= ForcedMoveArms.Arm25;
            else if (arm >= s_relayed.Length || !s_relayed[arm])
                return new MovementRead(default, null, MovementRefusal.UnconfirmedArm, forced);
            else if (IsFinite(command.Unknown1))
                relayable++;
        }

        Vector3? position = null;
        float? facing = null;
        Server0262Part5[]? commands = relayable > 0 ? new Server0262Part5[relayable] : null;
        int next = 0;
        foreach (Server0262Part5 command in message.Commands)
        {
            if (command.Unknown0 is KeyedPositions or KeyedStates || !IsFinite(command.Unknown1))
                continue;

            commands![next++] = command;
            switch (command.Unknown1)
            {
                case SetPlatformCommand platform:
                    onPlatform = platform.PlatformUnitId != 0;
                    break;
                case SetPositionCommand moved when !onPlatform:
                    position = moved.Position;
                    break;
                case SetRotationCommand turned when !onPlatform:
                    facing = turned.Rotation.X;
                    break;
            }
        }

        RelayBatch? batch = commands is null ? null : new RelayBatch(message.ClientTime, commands);
        return new MovementRead(new MovementSample(position, facing), batch, MovementRefusal.None, forced);
    }

    // A command carrying a value that is not finite is kept from the map and from every other client.
    private static bool IsFinite(IServer0262Union2 command) => command switch
    {
        SetPositionCommand moved => IsFinite(moved.Position),
        SetVelocityCommand velocity => IsFinite(velocity.Velocity),
        SetMoveCommand move => IsFinite(move.CommandedVelocity),
        SetRotationCommand turned => IsFinite(turned.Rotation),
        Server0262Union2Arm14 spin => IsFinite(spin.Rotation) && float.IsFinite(spin.SpinRate),
        _ => true,
    };

    private static bool IsFinite(Vector3 value) => float.IsFinite(value.X) && float.IsFinite(value.Y) && float.IsFinite(value.Z);

    private static bool[] Arms(params int[] arms)
    {
        bool[] relayed = new bool[32];
        foreach (int arm in arms)
            relayed[arm] = true;
        return relayed;
    }
}
