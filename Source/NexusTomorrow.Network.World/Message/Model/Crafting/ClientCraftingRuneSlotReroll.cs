using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ClientCraftingRuneSlotReroll, Direction.ClientToServer)]
public sealed partial class ClientCraftingRuneSlotReroll
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
    [UInt(32)] public uint SlotIndex { get; private set; }
    [UInt(5)] public RuneType Type { get; private set; }
}
