using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Sent on lua function GameLib::InitiatePTRCharacterCopy or /ptrcopy.
[WireMessage(GameMessageOpcode.ClientPtrCopy, Direction.ClientToServer)]
public sealed partial class ClientPtrCopy
{
    [NotOnWire] public ulong CharacterId { get; private set; }
}
