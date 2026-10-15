using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ClientPathScientistDismissScanbotPathAction)]
[WireMessage(GameMessageOpcode.ClientPathScientistDismissScanbotPathAction, Direction.ClientToServer)]
public sealed partial class ClientPathScientistDismissScanbotPathAction : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
