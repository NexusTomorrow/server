using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientCraftingRuneSlotReroll)]
[WireMessage(GameMessageOpcode.ClientCraftingRuneSlotReroll, Direction.ClientToServer)]
public sealed partial class ClientCraftingRuneSlotReroll : IReadable
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
    [UInt(32)] public uint SlotIndex { get; private set; }
    [UInt(5)] public RuneType Type { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ItemGuid = reader.ReadULong();
        SlotIndex = reader.ReadUInt();
        Type = reader.ReadEnum<RuneType>(5);
    }
}
