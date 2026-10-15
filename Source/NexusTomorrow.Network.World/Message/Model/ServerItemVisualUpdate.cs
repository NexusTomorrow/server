using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerItemVisualUpdate)]
[WireMessage(GameMessageOpcode.ServerItemVisualUpdate, Direction.ServerToClient)]
public sealed partial class ServerItemVisualUpdate : IWritable
{
    [UInt(32)] public uint Guid { get; set; }
    [Array(CountBits = 32)] public List<ItemVisual> ItemVisuals { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(ItemVisuals.Count);
        ItemVisuals.ForEach(v => v.Write(writer));
    }
}
