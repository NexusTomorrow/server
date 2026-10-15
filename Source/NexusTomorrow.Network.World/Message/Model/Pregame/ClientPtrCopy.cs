using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Sent on lua function GameLib::InitiatePTRCharacterCopy or /ptrcopy.
[Message(GameMessageOpcode.ClientPtrCopy)]
[WireMessage(GameMessageOpcode.ClientPtrCopy, Direction.ClientToServer)]
public sealed partial class ClientPtrCopy : IReadable
{
    [NotOnWire] public ulong CharacterId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
