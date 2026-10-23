using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Lists the players queued for a map; the client prints the list in chat.
/// </summary>
[WireMessage(GameMessageOpcode.Server060A, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MatchingQueuedPlayers
{
    [WireStruct]
    public sealed partial class Server060APart1
    {
        [UInt(16)] public ushort Unknown0 { get; set; }
        [UInt(8)] public byte Unknown1 { get; set; }
    }

    [WireStruct]
    public sealed partial class Server060APart2
    {
        [UInt(15)] public ushort Unknown0 { get; set; }
        [UInt(16)] public ushort Unknown1 { get; set; }
    }

    /// <summary>
    /// One queued player.
    /// </summary>
    [WireStruct]
    public sealed partial class Server060APart0
    {
        /// <summary>
        /// The player, by realm-scoped character id; the client prints the realm.
        /// </summary>
        public RealmScopedId PlayerId { get; set; } = new();

        /// <summary>
        /// The player's name.
        /// </summary>
        [WideString] public string Name { get; set; } = "";

        /// <summary>
        /// The player's faction, as a Faction2 id; the client prints 167 as the Exiles and any other value as the Dominion.
        /// </summary>
        [UInt(14)] public ushort Faction { get; set; }

        [UInt(14)] public uint Unknown3 { get; set; }
        [UInt(14)] public uint Unknown4 { get; set; }
        [UInt(2)] public byte Unknown5 { get; set; }

        /// <summary>
        /// The player's level.
        /// </summary>
        [UInt(32)] public uint Level { get; set; }

        [UInt(3)] public uint Unknown7 { get; set; }
        [UInt(32)] public uint Unknown8 { get; set; }
        [UInt(1)] public bool Unknown9 { get; set; }
        [UInt(32)] public uint Unknown10 { get; set; }
        [Float32] public float Unknown11 { get; set; }
        [FixedArray(5)] public Server060APart1[] Unknown12 { get; set; } = new Server060APart1[5];
        [Array(CountBits = 32)] public List<Server060APart2> Unknown13 { get; set; } = [];
    }

    /// <summary>
    /// The map, as a MatchingGameMap id.
    /// </summary>
    [UInt(32)] public uint GameMapId { get; set; }

    /// <summary>
    /// The players.
    /// </summary>
    [Array(CountBits = 32)] public List<Server060APart0> Players { get; set; } = [];
}
