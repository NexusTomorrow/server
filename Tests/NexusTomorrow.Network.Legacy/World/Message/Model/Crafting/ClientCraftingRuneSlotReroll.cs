using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientCraftingRuneSlotReroll)]
public class ClientCraftingRuneSlotReroll : IReadable
{
    public ulong ItemGuid { get; private set; }
    public uint SlotIndex { get; private set; }
    public RuneType Type { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ItemGuid = reader.ReadULong();
        SlotIndex = reader.ReadUInt();
        Type = reader.ReadEnum<RuneType>(5);
    }
}
