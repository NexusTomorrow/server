using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting
{
    [Message(GameMessageOpcode.ClientCraftingSimpleCraft)]
    public class ClientCraftingSimpleCraft : IReadable
    {
        public uint ClientSpellcastUniqueId { get; private set; }
        public uint CraftingStationUnitId { get; private set; }
        public uint TradeskillSchematic2Id { get; private set; }

        public void Read(GamePacketReader reader)
        {
            ClientSpellcastUniqueId = reader.ReadUInt();
            CraftingStationUnitId = reader.ReadUInt();
            TradeskillSchematic2Id = reader.ReadUInt();
        }
    }
}
