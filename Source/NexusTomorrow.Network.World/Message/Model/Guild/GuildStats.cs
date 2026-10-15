using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class GuildStats : IWritable
{
    [UInt(16)] public ushort MemberCount { get; set; }
    [UInt(16)] public ushort PerkCount { get; set; }
    [UInt(3)] public GuildClassification Classification { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MemberCount);
        writer.Write(PerkCount);
        writer.Write(Classification, 3u);
    }
}
