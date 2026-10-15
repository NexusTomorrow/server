using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
[UnionCase(EntityType.Player)]
public sealed partial class PlayerEntityModel : IEntityModel
{
    [UInt(64)] public ulong Id { get; set; }
    [UInt(14)] public ushort RealmId { get; set; }
    [WideString] public string Name { get; set; }
    [UInt(5)] public Race Race { get; set; }
    [UInt(5)] public Class Class { get; set; }
    [UInt(2)] public Sex Sex { get; set; }
    [UInt(64)] public ulong GroupId { get; set; }
    [Array(CountBits = 8), UInt(32)] public List<uint> PetIdList { get; } = [];
    [WideString] public string GuildName { get; set; }
    [UInt(4)] public GuildType GuildType { get; set; }
    [Array(CountBits = 5), UInt(64)] public List<ulong> GuildIds { get; set; } = []; // Only appears in sniffs when user has a guild, assume related to guild as well. Guild members?
    [Array(CountBits = 6), Float32] public List<float> Bones { get; set; } = [];
    [UInt(3)] public PvPFlag PvPFlag { get; set; }
    [UInt(8)] public byte Unknown4C { get; set; }
    [UInt(14)] public ushort Title { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Id);
        writer.Write(RealmId, 14);
        writer.WriteStringWide(Name);
        writer.Write(Race, 5);
        writer.Write(Class, 5);
        writer.Write(Sex, 2);
        writer.Write(GroupId);

        writer.Write((byte)PetIdList.Count);
        PetIdList.ForEach(e => writer.Write(e));

        writer.WriteStringWide(GuildName);
        writer.Write(GuildType, 4u);

        writer.Write((byte)GuildIds.Count, 5);
        GuildIds.ForEach(e => writer.Write(e));

        writer.Write(Bones.Count, 6);
        Bones.ForEach(e => writer.Write(e));

        writer.Write(PvPFlag, 3);
        writer.Write(Unknown4C);
        writer.Write(Title, 14);
    }
}
