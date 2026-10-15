using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
public sealed partial class GhostEntityModel : IEntityModel
{
    [UInt(64)] public ulong Id { get; set; }
    [UInt(14)] public ushort RealmId { get; set; }
    [WideString] public string Name { get; set; }
    [UInt(5)] public Race Race { get; set; }
    [UInt(5)] public Class Class { get; set; }
    [UInt(2)] public Sex Sex { get; set; }
    [UInt(64)] public ulong GroupId { get; set; }
    [WideString] public string GuildName { get; set; }
    [UInt(4)] public GuildType GuildType { get; set; }
    [Array(CountBits = 5), UInt(64)] public List<ulong> GuildIds { get; set; } = [];
    [Array(CountBits = 6), Float32] public List<float> Bones { get; set; } = [];
    [UInt(14)] public ushort Title { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Id);
        writer.Write(RealmId, 14u);
        writer.WriteStringWide(Name);
        writer.Write(Race, 5u);
        writer.Write(Class, 5u);
        writer.Write(Sex, 2u);
        writer.Write(GroupId);

        writer.WriteStringWide(GuildName);
        writer.Write(GuildType, 4u);

        writer.Write((byte)GuildIds.Count, 5u);
        GuildIds.ForEach(e => writer.Write(e));

        writer.Write(Bones.Count, 6u);
        Bones.ForEach(e => writer.Write(e));

        writer.Write(Title, 14u);
    }
}
