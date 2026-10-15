using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingFlagsUpdate)]
[WireMessage(GameMessageOpcode.ClientHousingFlagsUpdate, Direction.ClientToServer)]
public sealed partial class ClientHousingFlagsUpdate : IReadable
{
    public Identity ResidenceIdentity { get; } = new();
    [UInt(32)] public ResidenceFlags Flags { get; private set; }
    [UInt(3)] public byte NeighbourHarvestSplit { get; private set; } // can never be more than 5
    [UInt(3)] public byte NeighbourGardenSplit { get; private set; } // can never be more than 5

    public void Read(GamePacketReader reader)
    {
        ResidenceIdentity.Read(reader);
        Flags = reader.ReadEnum<ResidenceFlags>(32u);
        NeighbourHarvestSplit = reader.ReadByte(3u);
        NeighbourGardenSplit = reader.ReadByte(3u);
    }
}
