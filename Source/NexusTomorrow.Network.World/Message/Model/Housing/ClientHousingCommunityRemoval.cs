using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingCommunityRemoval)]
[WireMessage(GameMessageOpcode.ClientHousingCommunityRemoval, Direction.ClientToServer)]
public sealed partial class ClientHousingCommunityRemoval : IReadable
{
    public Identity TargetResidence { get; private set; } = new(); // This is only ever the player's own residence

    public void Read(GamePacketReader reader)
    {
        TargetResidence.Read(reader);
    }
}
