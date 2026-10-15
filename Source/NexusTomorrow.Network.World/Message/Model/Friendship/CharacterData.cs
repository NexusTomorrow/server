using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireStruct]
public sealed partial class CharacterData : IWritable
{
    [WideString] public string Name { get; set; } = "";
    public Identity PlayerIdentity { get; set; } = new();
    [UInt(8)] public Class Class { get; set; }
    [Pad(6)] public Pad ClassHigh { get; }
    [UInt(8)] public Race Race { get; set; }
    [Pad(6)] public Pad RaceHigh { get; }
    [UInt(8)] public Game.Static.PlayerPath.Path Path { get; set; }
    [Pad(24)] public Pad PathHigh { get; }
    [UInt(32)] public uint Level { get; set; }
    [UInt(15)] public ushort WorldZoneId { get; set; }
    [UInt(14)] public Faction Faction { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.WriteStringWide(Name);
        PlayerIdentity.Write(writer);
        writer.Write(Class, 14u);
        writer.Write(Race, 14u);
        writer.Write(Path, 32);
        writer.Write(Level);
        writer.Write(WorldZoneId, 15u);
        writer.Write(Faction, 14u);
    }
}

