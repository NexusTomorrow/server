using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ClientCraftingRuneSlotAdd)]
[WireMessage(GameMessageOpcode.ClientCraftingRuneSlotAdd, Direction.ClientToServer)]
public sealed partial class ClientCraftingRuneSlotAdd : IReadable
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
    [UInt(1)] public bool IsNotFusion { get; private set; }
    [UInt(5)] public RuneType Type { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ItemGuid = reader.ReadULong();
        IsNotFusion = reader.ReadBit();
        Type = reader.ReadEnum<RuneType>(5);
    }
}
