using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.ClientCraftingRuneInstall, Direction.ClientToServer)]
public sealed partial class ClientCraftingRuneInstall
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
    [Array(CountBits = 32), UInt(32)] public uint[] RuneSlotItem2Id { get; private set; } // Item2Id of rune, arranged in order of slot
}
