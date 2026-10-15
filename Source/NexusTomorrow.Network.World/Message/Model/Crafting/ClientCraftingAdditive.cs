using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientCraftingAdditive)]
[WireMessage(GameMessageOpcode.ClientCraftingAdditive, Direction.ClientToServer)]
public sealed partial class ClientCraftingAdditive : IReadable
{
    [UInt(32)] public uint CraftingStationUnitId { get; private set; }
    [UInt(18)] public uint AdditiveItem2Id { get; private set; }
    [UInt(18)] public uint CatalystItem2Id { get; private set; }

    public void Read(GamePacketReader reader)
    {
        CraftingStationUnitId = reader.ReadUInt();
        AdditiveItem2Id = reader.ReadUInt(18);
        CatalystItem2Id = reader.ReadUInt(18);
    }
}
