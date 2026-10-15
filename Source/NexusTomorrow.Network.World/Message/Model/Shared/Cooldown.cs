using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Cooldown : IWritable
{
    [UInt(3)] public byte Type { get; set; }
    [UInt(18)] public uint SpellId { get; set; }
    [UInt(32)] public uint TypeId { get; set; }
    [UInt(32)] public uint TimeRemaining { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Type, 3u);
        writer.Write(SpellId, 18u);
        writer.Write(TypeId);
        writer.Write(TimeRemaining);
    }
}
