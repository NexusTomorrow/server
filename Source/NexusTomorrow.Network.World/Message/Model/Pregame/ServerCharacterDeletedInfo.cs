using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Not a very useful message but included for completeness.
// Client only reads this message when on the CharacaterSelect screen.
// Data is accessible by calling lua function CharacterScreenLib::GetDeletedCharacterInfo
// which is not used by the Carbine UI. Requires custom CharacterSelect screen to be useful.
[Message(GameMessageOpcode.ServerCharacterDeletedInfo)]
[WireMessage(GameMessageOpcode.ServerCharacterDeletedInfo, Direction.ServerToClient)]
public sealed partial class ServerCharacterDeletedInfo : IWritable
{
    [WireStruct]
    public sealed partial class DeletedCharacterInfo : IWritable
    {
        [UInt(64)] public ulong CharacterId { get; set; }
        [UInt(32)] public uint Unused { get; set; }
        [WideString] public string Name { get; set; }
        [UInt(32)] public uint Level { get; set; }
        [UInt(14)] public ushort Class { get; set; }
        [UInt(14)] public ushort Race { get; set; }
        [UInt(2)] public ushort Gender { get; set; }
        [UInt(14)] public ushort Faction { get; set; }
        [UInt(3)] public ushort Path { get; set; }
        [UInt(15)] public ushort WorldId { get; set; }
        [UInt(15)] public ushort WorldZoneId { get; set; }
        [Float32] public float PurgeDate { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(CharacterId);
            writer.Write(Unused);
            writer.WriteStringWide(Name);
            writer.Write(Level);
            writer.Write(Class, 14u);
            writer.Write(Race, 14u);
            writer.Write(Gender, 2u);
            writer.Write(Faction, 14u);
            writer.Write(Path, 3u);
            writer.Write(WorldId, 15u);
            writer.Write(WorldZoneId, 15u);
            writer.Write(PurgeDate);
        }
    }

    [Array(CountBits = 32)] public List<DeletedCharacterInfo> DelectedCharacters { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DelectedCharacters.Count);
        DelectedCharacters.ForEach(m => m.Write(writer));
    }
}
