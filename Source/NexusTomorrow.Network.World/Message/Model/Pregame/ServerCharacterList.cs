using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.ServerCharacterList, Direction.ServerToClient)]
public sealed partial class ServerCharacterList
{
    [WireStruct]
    public sealed partial class Character
    {
        // probably, x, y, z, yaw, pitch
        [WireStruct]
        public sealed partial class WorldLocation
        {
            public Position Position { get; set; } = new Position();
            [Float32] public float Yaw { get; set; }
            [Float32] public float Pitch { get; set; }
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
    }

    [UInt(64)] public ulong ServerTime { get; set; }
    [Array(CountBits = 32)] public List<Character> Characters { get; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> EnabledCharacterCreationIds { get; set; } = []; // Unclear if sending these has any effect.
    [Array(CountBits = 32), UInt(32)] public List<uint> DisabledCharacterCreationIds { get; set; } = [];
    [UInt(14)] public ushort RealmId { get; set; }
    public Identity CharacterRemoveIdentity { get; set; } = new Identity();
    [UInt(32)] public uint CharacterRemoveTime { get; set; }
    [UInt(32)] public uint CharacterReservationCount { get; set; }
    [UInt(32)] public uint MaxNumberCharacters { get; set; }
    [UInt(32)] public uint AdditionalCount { get; set; }
    [UInt(14)] public Faction FactionRestriction { get; set; }
    [UInt(1)] public bool FreeLevel50 { get; set; }
}
