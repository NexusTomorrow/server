using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Sent when the lua function CharacterScreenLib::ExitToRealmSelect is called
[Message(GameMessageOpcode.ClientRealmList)]
public class ClientRealmList : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte messages
    }
}
