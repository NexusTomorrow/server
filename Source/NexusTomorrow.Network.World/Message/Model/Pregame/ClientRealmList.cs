using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Sent when the lua function CharacterScreenLib::ExitToRealmSelect is called
[Message(GameMessageOpcode.ClientRealmList)]
[WireMessage(GameMessageOpcode.ClientRealmList, Direction.ClientToServer)]
public sealed partial class ClientRealmList : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte messages
    }
}
