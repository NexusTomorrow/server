using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Model;
using NexusTomorrow.Network.World.Message.Model.Shared;
using NexusTomorrow.Network.World.Message.Model.Spell;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Creates a unit in the client's world under an id the client then knows it by: its type and the data that type carries, its stats and
/// other values set by id, the commands applied to it, the spells created on it, its faction, and its display and outfit.
/// </summary>
/// <remarks>
/// The client ignores a create for an id it already has, and that unit keeps what it had. Among the units it refuses are a unit whose type
/// (<see cref="Unknown1"/>) is 21, 26 or 28, a unit with a command it cannot apply, a unit that names a creature the client does not have
/// unless its type is 10 or 16, a type-20 unit whose <see cref="Server0262Union0Arm14.Unknown0"/> or
/// <see cref="Server0262Union0Arm14.Unknown1"/> is 0 or whose <see cref="Server0262Union0Arm14.Unknown3"/> is above 17, and a type-23 unit
/// whose <see cref="Server0262Union0Arm17.Unknown3"/> is above 17. The player's unit comes from this message before
/// <see cref="PlayerUnit"/> names it, and the client refuses that <see cref="PlayerUnit"/> unless <see cref="Unknown12"/> is an id in the
/// client's Faction2 table. If
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
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The unit's type, 0 to 36, which selects the data in <see cref="Unknown2"/>. The client refuses types 21, 26 and 28.
    /// </summary>
    [UInt(6)] public byte Unknown1 { get; set; }

    /// <summary>
    /// The data of the unit's type (<see cref="Unknown1"/>).
    /// </summary>
    [Union(nameof(Unknown1))] public IServer0262Union0 Unknown2 { get; set; } = new Server0262Union0Arm00();

    [UInt(8)] public byte Unknown3 { get; set; }

    /// <summary>
    /// The unit's stats, each under its stat id, sent as an integer, a float or a pair of integers. The unit's health is 0 unless stat 0
    /// sets it.
    /// </summary>
    [Array(CountBits = 5)] public List<Server0262Part4> Unknown4 { get; set; } = [];

    /// <summary>
    /// A value the client pairs with its own clock when it creates the unit.
    /// </summary>
    [UInt(32)] public uint Unknown5 { get; set; }

    /// <summary>
    /// Commands the client applies to the unit as it creates it. If it cannot apply one, it refuses the unit.
    /// </summary>
    [Array(CountBits = 5)] public List<Server0262Part5> Unknown6 { get; set; } = [];

    /// <summary>
    /// Values the client sets on the unit, each a pair under an id from 0 to 196. The second value under id 7 is the unit's maximum health.
    /// The client does not take an id above 196.
    /// </summary>
    [Array(CountBits = 8)] public List<Server0262Part6> Unknown7 { get; set; } = [];

    [Array(CountBits = 7)] public List<Server0117Part1> Unknown8 { get; set; } = [];

    /// <summary>
    /// Spells the client creates on the unit. A spell it fails to create, or cannot find once created, does not refuse the unit.
    /// </summary>
    [Array(CountBits = 9)] public List<Server0262Part7> Unknown9 { get; } = [];

    /// <summary>
    /// A spell, as an id the client looks up among the spells it holds.
    /// </summary>
    [UInt(32)] public uint Unknown10 { get; set; }

    /// <summary>
    /// The unit's faction, as an id in the client's Faction2 table. It may differ from the base faction, <see cref="Unknown12"/>.
    /// </summary>
    [UInt(14)] public ushort Unknown11 { get; set; }

    /// <summary>
    /// The unit's base faction, as an id in the client's Faction2 table. The client refuses <see cref="PlayerUnit"/> for the unit unless
    /// this id is in that table.
    /// </summary>
    [UInt(14)] public ushort Unknown12 { get; set; }

    [UInt(32)] public uint Unknown13 { get; set; }
    [UInt(64)] public ulong Unknown14 { get; set; }
    public Server0262Part14 Unknown15 { get; set; } = new();
    public Server0262Part15 Unknown16 { get; set; } = new();
    public Server0262Part16 Unknown17 { get; set; } = new();
    [UInt(14)] public ushort Unknown18 { get; set; }

    /// <summary>
    /// The unit's display, as an id in the client's Creature2DisplayInfo table, or 0 for none. The client ignores an id the table lacks.
    /// </summary>
    [UInt(17)] public uint Unknown19 { get; set; }

    /// <summary>
    /// The unit's outfit, as an id in the client's Creature2OutfitInfo table, or 0 for none. The client ignores an id the table lacks.
    /// </summary>
    [UInt(15)] public ushort Unknown20 { get; set; }
}
