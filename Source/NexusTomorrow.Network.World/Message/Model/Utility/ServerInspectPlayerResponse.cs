using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[Message(GameMessageOpcode.ServerInspectPlayerResponse)]
[WireMessage(GameMessageOpcode.ServerInspectPlayerResponse, Direction.ServerToClient)]
public sealed partial class ServerInspectPlayerResponse : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [Array(CountBits = 5)] public List<Item> Items { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);

        writer.Write(Items.Count, 5u);
        Items.ForEach(item => item.Write(writer));
    }
}
