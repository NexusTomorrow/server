using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The highest character level the account has reached.
/// </summary>
[WireMessage(GameMessageOpcode.Server0036, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MaxCharacterLevel
{
    /// <summary>
    /// The level. The client offers only the character creation options whose starting level is at most this, or at most 1 when
    /// it is 0.
    /// </summary>
    [UInt(32)] public uint Level { get; set; }
}
