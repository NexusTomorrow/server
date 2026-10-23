using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Model;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Creates a unit in the client's world under an id the client then knows it by: its type and the data that type carries, its stats and
/// other values set by id, the commands applied to it, the spells created on it, its faction, and its display and outfit.
/// </summary>
/// <remarks>
/// The client ignores a create for an id it already has, and that unit keeps what it had. Among the units it refuses are a unit whose type
/// (<see cref="UnitType"/>) is 21, 26 or 28, a unit with a command it cannot apply, a unit that names a creature the client does not have
/// unless its type is 10 or 16, a type-20 unit whose <see cref="Server0262Union0Arm14.CharacterId"/> or
/// <see cref="Server0262Union0Arm14.RealmId"/> is 0 or whose <see cref="Server0262Union0Arm14.RaceId"/> is above 17, and a type-23 unit
/// whose <see cref="Server0262Union0Arm17.Unknown3"/> is above 17. The player's unit comes from this message before
/// <see cref="PlayerUnit"/> names it, and the client refuses that <see cref="PlayerUnit"/> unless <see cref="BaseFactionId"/> is an id in
/// the client's Faction2 table. If
/// <see cref="MovementControl"/> named the id before the unit existed, the client takes control of the unit as it creates it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0262, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class UnitCreate
{
    [WireStruct]
    public sealed partial class Server0262Part14
    {
        public interface IServer0262Union4 : IWireStruct
        {
        }

        [WireStruct]
        [UnionCase(0)]
        public sealed partial class Server0262Union4Arm00 : IServer0262Union4
        {
            [UInt(1)] public bool Unknown0 { get; set; }
        }

        [WireStruct]
        [UnionCase(1)]
        public sealed partial class Server0262Union4Arm01 : IServer0262Union4
        {
            [UInt(17)] public uint Unknown0 { get; set; }
        }

        [UInt(2)] public byte Unknown0 { get; set; }
        [Union(nameof(Unknown0))] public IServer0262Union4 Unknown1 { get; set; } = new Server0262Union4Arm00();
    }

    [WireStruct]
    public sealed partial class Server0262Part15
    {
        public interface IServer0262Union5 : IWireStruct
        {
        }

        [WireStruct]
        [UnionCase(0)]
        public sealed partial class Server0262Union5Arm00 : IServer0262Union5
        {
            [UInt(1)] public bool Unknown0 { get; set; }
        }

        [WireStruct]
        [UnionCase(1)]
        public sealed partial class Server0262Union5Arm01 : IServer0262Union5
        {
            [UInt(64)] public ulong Unknown0 { get; set; }
            [UInt(14)] public ushort Unknown1 { get; set; }
        }

        [UInt(2)] public byte Unknown0 { get; set; }
        [Union(nameof(Unknown0))] public IServer0262Union5 Unknown1 { get; set; } = new Server0262Union5Arm00();
    }

    [WireStruct]
    public sealed partial class Server0262Part16
    {
        public interface IServer0262Union6 : IWireStruct
        {
        }

        [WireStruct]
        [UnionCase(0)]
        public sealed partial class Server0262Union6Arm00 : IServer0262Union6
        {
            [UInt(1)] public bool Unknown0 { get; set; }
        }

        [WireStruct]
        [UnionCase(1)]
        public sealed partial class Server0262Union6Arm01 : IServer0262Union6
        {
            [UInt(32)] public uint Unknown0 { get; set; }
            [UInt(18)] public uint Unknown1 { get; set; }
        }

        [UInt(2)] public byte Unknown0 { get; set; }
        [Union(nameof(Unknown0))] public IServer0262Union6 Unknown1 { get; set; } = new Server0262Union6Arm00();
    }

    /// <summary>
    /// The unit's id. The client keeps one unit per id, and other messages name the unit by it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's type, 0 to 36, which selects the data in <see cref="TypeData"/>. The client refuses types 21, 26 and 28.
    /// </summary>
    [VerifiedMember, UInt(6)] public byte UnitType { get; set; }

    /// <summary>
    /// The data of the unit's type (<see cref="UnitType"/>).
    /// </summary>
    [VerifiedMember, Union(nameof(UnitType))] public IServer0262Union0 TypeData { get; set; } = new Server0262Union0Arm00();

    [UInt(8)] public byte Unknown3 { get; set; }

    /// <summary>
    /// The unit's stats, each under its stat id, sent as an integer, a float or a pair of integers. The unit's health is 0 unless stat 0
    /// sets it.
    /// </summary>
    [VerifiedMember, Array(CountBits = 5)] public List<Server0262Part4> Stats { get; set; } = [];

    /// <summary>
    /// A value the client pairs with its own clock when it creates the unit.
    /// </summary>
    [UInt(32)] public uint Unknown5 { get; set; }

    /// <summary>
    /// Commands the client applies to the unit as it creates it. If it cannot apply one, it refuses the unit.
    /// </summary>
    [VerifiedMember, Array(CountBits = 5)] public List<Server0262Part5> Commands { get; set; } = [];

    /// <summary>
    /// Values the client sets on the unit, each a base and a current value under a property id from 0 to 196. The client first resets
    /// every property to its default and then applies these in order, skipping an id above 196. The current value of property 7 is the
    /// unit's maximum health and of property 38 its maximum dash energy.
    /// </summary>
    [VerifiedMember, Array(CountBits = 8)] public List<UnitPropertyValue> Properties { get; set; } = [];

    /// <summary>
    /// The items the unit shows, each giving one slot of its model its look and colour as <see cref="UnitItemVisualsChanged.Items"/>
    /// does.
    /// </summary>
    [VerifiedMember, Array(CountBits = 7)] public List<ItemVisual> Items { get; set; } = [];

    /// <summary>
    /// Spells the client creates on the unit. A spell it fails to create, or cannot find once created, does not refuse the unit.
    /// </summary>
    [Array(CountBits = 9)] public List<Server0262Part7> Spells { get; } = [];

    /// <summary>
    /// A spell, as an id the client looks up among the spells it holds.
    /// </summary>
    [UInt(32)] public uint SpellId { get; set; }

    /// <summary>
    /// The unit's faction, as an id in the client's Faction2 table. It may differ from the base faction, <see cref="BaseFactionId"/>.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort FactionId { get; set; }

    /// <summary>
    /// The unit's base faction, as an id in the client's Faction2 table. The client refuses <see cref="PlayerUnit"/> for the unit unless
    /// this id is in that table.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort BaseFactionId { get; set; }

    /// <summary>
    /// The unit that tagged this one, by unit id. Unit:IsTaggedByMe is true when it is the player's unit, and Unit:IsTagged tests it
    /// with <see cref="TaggedByGroupId"/>. With both at 0 the client also clears the unit's tagged-by-me flag, which Unit:IsTaggedByMe
    /// reads too.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint TaggedByUnitId { get; set; }

    /// <summary>
    /// The group that tagged the unit, by group id. Unit:IsTaggedByMe is true when it is the player's group, and Unit:IsTagged tests it
    /// with <see cref="TaggedByUnitId"/>.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong TaggedByGroupId { get; set; }

    public Server0262Part14 Unknown15 { get; set; } = new();
    public Server0262Part15 Unknown16 { get; set; } = new();
    public Server0262Part16 Unknown17 { get; set; } = new();

    /// <summary>
    /// The unit's minimap marker, by id. Unit:GetMiniMapMarkers returns it, or else the minimap marker of the creature's
    /// TradeskillHarvestingInfo row for a tier the player knows. <see cref="UnitMiniMapMarkerChanged"/> sets it too.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort MiniMapMarkerId { get; set; }

    /// <summary>
    /// The unit's display, as an id in the client's Creature2DisplayInfo table, or 0 for none. The client ignores an id the table lacks.
    /// </summary>
    [UInt(17)] public uint DisplayInfoId { get; set; }

    /// <summary>
    /// The unit's outfit, as an id in the client's Creature2OutfitInfo table, or 0 for none. The client ignores an id the table lacks.
    /// </summary>
    [UInt(15)] public ushort OutfitInfoId { get; set; }
}
