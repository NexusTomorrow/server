using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ClientCraftingAdditive, Direction.ClientToServer)]
public sealed partial class ClientCraftingAdditive
{
    [UInt(32)] public uint CraftingStationUnitId { get; private set; }
    [UInt(18)] public uint AdditiveItem2Id { get; private set; }
    [UInt(18)] public uint CatalystItem2Id { get; private set; }
}
