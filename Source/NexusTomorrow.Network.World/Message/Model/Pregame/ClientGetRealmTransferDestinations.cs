using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Sent when the lua function CharacterScreenLib::GetRealmTransferDestinations is called
// Response from ServerTransferDestinationRealmList
[Message(GameMessageOpcode.ClientGetRealmTransferDestinations)]
[WireMessage(GameMessageOpcode.ClientGetRealmTransferDestinations, Direction.ClientToServer)]
public sealed partial class ClientGetRealmTransferDestinations : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte messages
    }
}
