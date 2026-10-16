using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The outcome of a realm transfer, which the client hands to the character-select screen's interface.
/// </summary>
[WireMessage(GameMessageOpcode.Server0765, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class RealmTransferResult
{
    /// <summary>
    /// The result. 21 means the transfer went through, and the interface treats every other value as an error. It has a message of its
    /// own for 20 to 33, 47, 48, 51 and 55.
    /// </summary>
    [UInt(6)] public byte Result { get; set; }
}
