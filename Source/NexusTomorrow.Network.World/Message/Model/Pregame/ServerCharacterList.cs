using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ServerCharacterList)]
public partial class ServerCharacterList : IWritable
{
    [WireStruct]
    public sealed partial class Character : IWritable
    {
        // probably, x, y, z, yaw, pitch
        [WireStruct]
        public sealed partial class WorldLocation : IWritable
        {
            public Position Position { get; set; } = new Position();
            [Float32] public float Yaw { get; set; }
            [Float32] public float Pitch { get; set; }

            public void Write(GamePacketWriter writer)
            {
                Position.Write(writer);
                writer.Write(Yaw);
                writer.Write(Pitch);
            }
        }

        [UInt(64)] public ulong Id { get; set; }
        [WideString] public string Name { get; set; }
        [UInt(2)] public Sex Sex { get; set; }
        [UInt(5)] public Race Race { get; set; }
        [UInt(5)] public Class Class { get; set; }
        [UInt(32)] public uint Faction { get; set; }
        [UInt(32)] public uint Level { get; set; }
        [Array(CountBits = 32)] public List<ItemVisual> Appearance { get; } = [];
        [Array(CountBits = 32)] public List<ItemVisual> Gear { get; } = [];
        [UInt(15)] public ushort WorldId { get; set; }
        [UInt(15)] public ushort WorldZoneId { get; set; }
        [UInt(14)] public ushort RealmId { get; set; }
        public WorldLocation Location { get; } = new();
        [UInt(3)] public byte Path { get; set; }
        [UInt(1)] public bool IsLocked { get; set; }
        [UInt(1)] public bool RequiresRename { get; set; }
        [UInt(32)] public uint GearMask { get; set; }
        [UInt(4), Count] public byte CustomisationCount { get; set; }
        [Array(CountFrom = nameof(CustomisationCount)), UInt(32)] public List<uint> Labels { get; } = [];
        [Array(CountFrom = nameof(CustomisationCount)), UInt(32)] public List<uint> Values { get; } = [];
        [Array(CountBits = 32), Float32] public List<float> Bones { get; } = [];
        [Float32] public float LastLoggedOutDays { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Id);
            writer.WriteStringWide(Name);
            writer.Write(Sex, 2);
            writer.Write(Race, 5);
            writer.Write(Class, 5);
            writer.Write(Faction);
            writer.Write(Level);

            writer.Write(Appearance.Count);
            foreach (ItemVisual item in Appearance)
                item.Write(writer);

            writer.Write(Gear.Count);
            foreach (ItemVisual item in Gear)
                item.Write(writer);

            writer.Write(WorldId, 15);
            writer.Write(WorldZoneId, 15);
            writer.Write(RealmId, 14);

            Location.Write(writer);

            writer.Write(Path, 3);
            writer.Write(IsLocked);
            writer.Write(RequiresRename);
            writer.Write(GearMask);

            writer.Write(Labels.Count, 4);
            for (int i = 0; i < Labels.Count; i++)
                writer.Write(Labels[i]);
            for (int i = 0; i < Labels.Count; i++)
                writer.Write(Values[i]);

            writer.Write(Bones.Count);
            foreach (float value in Bones)
                writer.Write(value);

            writer.Write(LastLoggedOutDays);
        }
    }

    public ulong ServerTime { get; set; }
    public List<Character> Characters { get; } = [];
    public List<uint> EnabledCharacterCreationIds { get; set; } = []; // Unclear if sending these has any effect.
    public List<uint> DisabledCharacterCreationIds { get; set; } = [];
    public ushort RealmId { get; set; }
    public Identity CharacterRemoveIdentity { get; set; } = new Identity();
    public uint CharacterRemoveTime { get; set; }
    public uint CharacterReservationCount { get; set; }
    public uint MaxNumberCharacters { get; set; }
    public uint AdditionalCount { get; set; }
    public Faction FactionRestriction { get; set; }
    public bool FreeLevel50 { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ServerTime);
        writer.Write(Characters.Count);

        foreach (Character character in Characters)
            character.Write(writer);

        writer.Write(EnabledCharacterCreationIds.Count);
        foreach (uint value in EnabledCharacterCreationIds)
            writer.Write(value);

        writer.Write(DisabledCharacterCreationIds.Count);
        foreach (uint value in DisabledCharacterCreationIds)
            writer.Write(value);

        writer.Write(RealmId, 14);

        CharacterRemoveIdentity.Write(writer);

        writer.Write(CharacterRemoveTime);
        writer.Write(CharacterReservationCount);
        writer.Write(MaxNumberCharacters);
        writer.Write(AdditionalCount);
        writer.Write(FactionRestriction, 14);
        writer.Write(FreeLevel50);
    }
}
