using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientCraftingRuneInstall)]
[WireMessage(GameMessageOpcode.ClientCraftingRuneInstall, Direction.ClientToServer)]
public sealed partial class ClientCraftingRuneInstall : IReadable
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
    [Array(CountBits = 32), UInt(32)] public uint[] RuneSlotItem2Id { get; private set; } // Item2Id of rune, arranged in order of slot

    public void Read(GamePacketReader reader)
    {
        ItemGuid = reader.ReadULong();
        uint count = reader.ReadUInt();
        RuneSlotItem2Id = new uint[count];
        for (int i = 0; i < count; i++)
        {
            RuneSlotItem2Id[i] = reader.ReadUInt();
        }
    }
}
