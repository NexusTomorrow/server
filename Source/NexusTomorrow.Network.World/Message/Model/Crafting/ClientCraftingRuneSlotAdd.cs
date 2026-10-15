using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ClientCraftingRuneSlotAdd, Direction.ClientToServer)]
public sealed partial class ClientCraftingRuneSlotAdd
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
    [UInt(1)] public bool IsNotFusion { get; private set; }
    [UInt(5)] public RuneType Type { get; private set; }
}
