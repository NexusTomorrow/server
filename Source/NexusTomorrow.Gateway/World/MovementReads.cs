using System.Collections.Immutable;
using System.Numerics;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Owners.World;

namespace NexusTomorrow.Gateway.World;

/// <summary>
/// What the server reads from a <see cref="ClientMovement"/>, read to <c>Purpose</c>: the members <see cref="Members"/> names, each
/// marked verified, and nothing else. The opcode table admits the message for that reason.
/// </summary>
public static class MovementReads
{
    /// <summary>
    /// Every member <see cref="TryLatest"/> reads.
    /// </summary>
    public static ImmutableArray<(Type Type, string Member)> Members { get; } =
    [
        (typeof(ClientMovement), nameof(ClientMovement.Unknown1)),
        (typeof(Server0262Part5), nameof(Server0262Part5.Unknown1)),
        (typeof(Server0262Union2Arm01), nameof(Server0262Union2Arm01.Unknown0)),
        (typeof(Server0262Union2Arm02), nameof(Server0262Union2Arm02.Unknown0)),
        (typeof(Server0262Union2Arm0E), nameof(Server0262Union2Arm0E.Unknown0)),
    ];

    /// <summary>
    /// Take the newest position and facing from <paramref name="message"/>'s commands, in the order they came. A platform command sets
    /// <paramref name="onPlatform"/> to whether its unit id is not 0; while it is set, positions and facings are relative to that unit
    /// and are not taken. The state carries across messages, because the client sends a platform command only when it changes.
    /// </summary>
    /// <returns>True when a position or a facing was found.</returns>
    public static bool TryLatest(ClientMovement message, ref bool onPlatform, out MovementSample sample)
    {
        Vector3? position = null;
        float? facing = null;
        foreach (Server0262Part5 command in message.Unknown1)
        {
            switch (command.Unknown1)
            {
                case Server0262Union2Arm01 platform:
                    onPlatform = platform.Unknown0 != 0;
                    break;
                case Server0262Union2Arm02 moved when !onPlatform:
                    position = moved.Unknown0;
                    break;
                case Server0262Union2Arm0E turned when !onPlatform:
                    facing = turned.Unknown0.X;
                    break;
            }
        }

        sample = new MovementSample(position, facing);
        return position is not null || facing is not null;
    }
}
