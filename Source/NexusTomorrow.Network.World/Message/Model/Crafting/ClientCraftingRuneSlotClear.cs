using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientCraftingRuneSlotClear)]
[WireMessage(GameMessageOpcode.ClientCraftingRuneSlotClear, Direction.ClientToServer)]
public sealed partial class ClientCraftingRuneSlotClear : IReadable
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
    [UInt(32)] public uint RuneSlotIndex { get; private set; }
    [UInt(1)] public bool RecoverRune { get; private set; } // clear slot = 0, recover = 1
    [UInt(1)] public bool UseGroupCurrency { get; private set; } // use credits = 0, use group currency = 1

    public void Read(GamePacketReader reader)
    {
        ItemGuid = reader.ReadULong();
        RuneSlotIndex = reader.ReadUInt();
        RecoverRune = reader.ReadBit();
        UseGroupCurrency = reader.ReadBit();
    }
}
