using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// A level that limits the character creation options the client offers.
/// </summary>
[WireMessage(GameMessageOpcode.Server0036, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CreationLevelLimit
{
    /// <summary>
    /// The level. Of the creation options that need no entitlement, the client offers only those whose starting level is at most this,
    /// or at most 1 when it is 0.
    /// </summary>
    [UInt(32)] public uint Level { get; set; }
}
