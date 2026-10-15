using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public partial class PublicEventStats : IWritable
{
    [FixedArray(4), UInt(8)] public byte[] Mask { get; set; } = new byte[4]; // 32 flags, least significant bit of each byte first
    [Array(CountBits = 5), UInt(32)] public List<uint> Values { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.WriteBytes(Mask);
        writer.Write(Values.Count, 5);

        foreach (uint value in Values)
            writer.Write(value);
    }
}
