using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingNeighborInviteResponse)]
[WireMessage(GameMessageOpcode.ClientHousingNeighborInviteResponse, Direction.ClientToServer)]
public sealed partial class ClientHousingNeighbourInviteResponse : IReadable
{
    [UInt(1)] public bool Accept { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Accept = reader.ReadBit();
    }
}
