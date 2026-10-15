using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingRequestCommunityPlacement)]
[WireMessage(GameMessageOpcode.ClientHousingRequestCommunityPlacement, Direction.ClientToServer)]
public sealed partial class ClientHousingRequestCommunityPlacement : IReadable
{
    public Identity ResidenceIdentity { get; } = new();
    [UInt(32)] public uint PropertyIndex { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ResidenceIdentity.Read(reader);
        PropertyIndex = reader.ReadUInt();
    }
}
