using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The outcome of a realm transfer, which the client hands to the character-select screen's interface.
/// </summary>
[WireMessage(GameMessageOpcode.Server0765, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class RealmTransferResult
{
    [UInt(6)] public byte Unknown0 { get; set; }
}
