using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// The player's own state: the amount of each currency, experience, item proficiencies, entitlements and other values the client keeps
/// for the player.
/// </summary>
/// <remarks>
/// Taken only after <see cref="PlayerUnit"/>; before it the client refuses it and keeps none of it. The client sends nothing in reply, and
/// does not need it to finish entering the world.
/// </remarks>
[WireMessage(GameMessageOpcode.Server025E, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PlayerState
{
    [WireStruct]
    public sealed partial class Server025EPart0
    {
        [WireStruct]
        public sealed partial class Server025EPart1
        {
            /// <summary>
            /// The faction, as an id in the client's Faction2 table.
            /// </summary>
            [UInt(14)] public ushort Unknown0 { get; set; }

            [Float32] public float Unknown1 { get; set; }
        }

        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [UInt(14)] public ushort Unknown0 { get; set; }

        /// <summary>
        /// The factions, each with its value.
        /// </summary>
        [Array(CountBits = 16)] public List<Server025EPart1> Unknown1 { get; set; } = [];
    }

    [WireStruct]
    public sealed partial class Server025EPart2
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(18)] public uint Unknown1 { get; set; }
        [UInt(5)] public byte Unknown2 { get; set; }
        [UInt(5)] public byte Unknown3 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server025EPart3
    {
        /// <summary>
        /// The entitlement's id.
        /// </summary>
        [UInt(14)] public ushort Unknown0 { get; set; }

        /// <summary>
        /// The entitlement's count.
        /// </summary>
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [Array(CountBits = 32)] public List<Server0111Part0> Unknown0 { get; } = [];

    /// <summary>
    /// The amount of each currency the player holds: entry n is the currency whose type is n + 1.
    /// </summary>
    [FixedArray(16), UInt(64)] public ulong[] Unknown1 { get; } = new ulong[16];

    /// <summary>
    /// The player's total experience. The client works out from it which level the total falls in.
    /// </summary>
    [UInt(32)] public uint Unknown2 { get; set; }

    /// <summary>
    /// The player's rest experience, as the client's interface shows it.
    /// </summary>
    [UInt(32)] public uint Unknown3 { get; set; }

    /// <summary>
    /// The player's item proficiencies: a mask the client tests against each entry's bit in its ItemProficiency table.
    /// </summary>
    [UInt(32)] public uint Unknown4 { get; set; }

    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }

    /// <summary>
    /// Which of the player's ability sets is active, by index, as the client's interface shows it.
    /// </summary>
    [UInt(3)] public byte Unknown7 { get; set; }

    /// <summary>
    /// A signed amount the client adds to a total it works out from the player's level.
    /// </summary>
    [SInt(16)] public short Unknown8 { get; set; }

    [UInt(32)] public uint Unknown9 { get; set; }

    /// <summary>
    /// Values the client keeps for the player, one for each faction listed.
    /// </summary>
    public Server025EPart0 Unknown10 { get; set; } = new();

    [Array(CountBits = 32)] public List<Server025EPart2> Unknown11 { get; } = [];

    /// <summary>
    /// Which of the player's key binding sets is in use, as the client's interface shows it: 3 or 4. The client takes any other value as 3.
    /// </summary>
    [UInt(32)] public uint Unknown12 { get; set; }

    [UInt(16)] public ushort Unknown13 { get; set; }

    /// <summary>
    /// The index of the costume the player wears, as the client's interface shows it. Until a value arrives the client holds 0xFFFFFFFF.
    /// </summary>
    [UInt(32)] public uint Unknown14 { get; set; }

    [UInt(32)] public uint Unknown15 { get; set; }

    /// <summary>
    /// A time tied to the player's PvP flag, as the client's interface reports it, sent in milliseconds from when the client takes this
    /// message.
    /// </summary>
    [UInt(32)] public uint Unknown16 { get; set; }

    [Array(CountBits = 6), UInt(16)] public List<ushort> Unknown17 { get; } = [];

    /// <summary>
    /// Values the client keeps, one for each index from 0 to 511. It takes only the entries that are not 0: an entry of 0 leaves the
    /// client's value as it was.
    /// </summary>
    [FixedArray(512), UInt(16)] public ushort[] Unknown18 { get; set; } = new ushort[512];

    [Float32] public float Unknown19 { get; set; }

    /// <summary>
    /// Whether the server is a PvP server, as the client's interface shows it.
    /// </summary>
    [UInt(1)] public bool Unknown20 { get; set; }

    [UInt(32)] public uint Unknown21 { get; set; }

    /// <summary>
    /// The character's entitlements, each an id and a count, as the client's interface shows them.
    /// </summary>
    [Array(CountBits = 32)] public List<Server025EPart3> Unknown22 { set; get; } = [];
}
