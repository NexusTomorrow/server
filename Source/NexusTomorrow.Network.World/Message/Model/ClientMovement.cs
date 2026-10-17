using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// The movement of the unit the client moves, as commands in batches the client stamps with its own clock.
/// </summary>
/// <remarks>
/// The client sends it while it moves a unit: <see cref="MovementControl"/> starts it and <see cref="MovementControlEnd"/> stops it. The
/// first one after <see cref="MovementControl"/> carries only a time and a position whose flag is clear. After that the client sends from
/// its movement timer, whose period is <see cref="WorldSettings.MovementInterval"/>, and also at once at some moments, such as just before
/// <c>0x00FB</c>. Each batch opens with a time: the client's clock when it gathered the batch, plus two periods of
/// <see cref="WorldSettings.MovementInterval"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Message0637, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class ClientMovement
{
    /// <summary>
    /// The client's clock when it sent the message, in milliseconds.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The commands, one batch after another; each batch opens with a time command.
    /// </summary>
    [Array(CountBits = 32)] public List<Server0262Part5> Unknown1 { get; } = [];
}
