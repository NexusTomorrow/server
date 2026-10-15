using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

// Can only be sent after being identified as OpponentUnitId in ServerDuelChallenge (0x896)
[Message(GameMessageOpcode.ClientDuelDecline)]
[WireMessage(GameMessageOpcode.ClientDuelDecline, Direction.ClientToServer)]
public sealed partial class ClientDuelDecline : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
