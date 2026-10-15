using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting
{
    [Message(GameMessageOpcode.ClientCraftingAdditive)]
    public class ClientCraftingAdditive : IReadable
    {
        public uint CraftingStationUnitId { get; private set; }
        public uint AdditiveItem2Id { get; private set; }
        public uint CatalystItem2Id { get; private set; }

        public void Read(GamePacketReader reader)
        {
            CraftingStationUnitId = reader.ReadUInt();
            AdditiveItem2Id = reader.ReadUInt(18);
            CatalystItem2Id = reader.ReadUInt(18);
        }
    }
}
