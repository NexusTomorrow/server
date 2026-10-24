using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// The movement of the unit the client moves, as commands in batches the client stamps with its own clock. Every one opens with a
/// <see cref="SetTimeCommand"/>, and the client puts no command in it other than <see cref="SetTimeCommand"/>,
/// <see cref="SetPlatformCommand"/>, <see cref="SetPositionCommand"/>, <see cref="SetVelocityCommand"/>, <see cref="SetMoveCommand"/>,
/// <see cref="SetRotationCommand"/>, <see cref="SetStateCommand"/>, <see cref="SetModeCommand"/> and commands 10, 13, 18 and 20. Walking
/// forward brings a <see cref="SetPositionCommand"/>, a <see cref="SetVelocityCommand"/>, a <see cref="SetMoveCommand"/> and a
/// <see cref="SetStateCommand"/>; walking back brings a <see cref="SetVelocityCommand"/>, a <see cref="SetMoveCommand"/> and a
/// <see cref="SetStateCommand"/>; turning in place brings a <see cref="SetRotationCommand"/>.
/// </summary>
/// <remarks>
/// The client sends it while it moves a unit: <see cref="MovementControl"/> starts it and <see cref="MovementControlEnd"/> stops it. The
/// first one after the client takes a unit over carries only a <see cref="SetTimeCommand"/> and a <see cref="SetPositionCommand"/> whose
/// <see cref="SetPositionCommand.Blend"/> is clear. After that the client sends from its movement timer, whose period is
/// <see cref="WorldSettings.MovementInterval"/>, and also at once at some moments, such as just before <c>0x00FB</c>. Each batch opens with a
/// <see cref="SetTimeCommand"/> whose time is the client's clock when it gathered the batch, plus two periods of
/// <see cref="WorldSettings.MovementInterval"/>. After a <see cref="MovementControlEnd"/> has taken control away and a
/// <see cref="MovementControl"/> grants it back, the first batch the client sends from its movement timer for that grant carries one
/// <see cref="SetPlatformCommand"/> and one <see cref="SetModeCommand"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Message0637, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class ClientMovement
{
    /// <summary>
    /// The client's own clock when it sends the message, in milliseconds, as the client keeps it: it applies no conversion.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint ClientTime { get; set; }

    /// <summary>
    /// The commands, one batch after another; each batch opens with a <see cref="SetTimeCommand"/>.
    /// </summary>
    [VerifiedMember, Array(CountBits = 32)] public List<Server0262Part5> Unknown1 { get; } = [];
}
