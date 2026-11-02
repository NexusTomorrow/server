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
    /// The client passes this outcome to its interface when a realm transfer finishes.
    /// </summary>
    [VerifiedMember, UInt(6)] public byte Result { get; set; }
}
