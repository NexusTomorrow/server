using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ClientCraftingRuneSlotClear, Direction.ClientToServer)]
public sealed partial class ClientCraftingRuneSlotClear
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
    [UInt(32)] public uint RuneSlotIndex { get; private set; }
    [UInt(1)] public bool RecoverRune { get; private set; } // clear slot = 0, recover = 1
    [UInt(1)] public bool UseGroupCurrency { get; private set; } // use credits = 0, use group currency = 1
}
