using NexusTomorrow.Network.Message;
using NexusTomorrow.Game.Static.Crafting;

namespace NexusTomorrow.Network.World.Message.Model.Crafting
{
    [Message(GameMessageOpcode.ClientCraftingRuneSlotAdd)]
    public class ClientCraftingRuneSlotAdd : IReadable
    {
        public ulong ItemGuid { get; private set; }
        public bool IsNotFusion { get; private set; }
        public RuneType Type { get; private set; }

        public void Read(GamePacketReader reader)
        {
            ItemGuid = reader.ReadULong();
            IsNotFusion = reader.ReadBit();
            Type = reader.ReadEnum<RuneType>(5);
        }
    }
}
