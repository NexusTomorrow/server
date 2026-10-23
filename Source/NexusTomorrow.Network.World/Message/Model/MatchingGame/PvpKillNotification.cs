using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Reports a kill in PvP; the client raises PvpKillNotification with the victim's and the killer's names, the reason, the killer's
/// class and the victim's team.
/// </summary>
[WireMessage(GameMessageOpcode.Server05E7, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PvpKillNotification
{
    /// <summary>
    /// A killer or a victim: a player, a creature, or one the client has no name for.
    /// </summary>
    public interface IServer05E7Union0 : IWireStruct
    {
    }

    /// <summary>
    /// A killer or a victim who is a player.
    /// </summary>
    [WireStruct]
    [UnionCase(0)]
    public sealed partial class Server05E7Union0Arm00 : IServer05E7Union0
    {
        /// <summary>
        /// The player, by realm-scoped character id; the client asks the server for the name when it does not have it.
        /// </summary>
        public RealmScopedId PlayerId { get; set; } = new();

        /// <summary>
        /// The player's class; the kill board uses it for the killer only.
        /// </summary>
        [UInt(5)] public byte Class { get; set; }
    }

    /// <summary>
    /// A killer or a victim that is a creature.
    /// </summary>
    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Server05E7Union0Arm01 : IServer05E7Union0
    {
        /// <summary>
        /// The creature, by unit id; the client takes the name from the unit when it has it.
        /// </summary>
        [UInt(32)] public uint UnitId { get; set; }

        /// <summary>
        /// The creature, as a Creature2 id; the client takes the name from that record when it does not know the unit.
        /// </summary>
        [UInt(18)] public uint CreatureId { get; set; }
    }

    /// <summary>
    /// A killer or a victim the client has no name for.
    /// </summary>
    [WireStruct]
    [UnionCase(2)]
    public sealed partial class Server05E7Union0Arm02 : IServer05E7Union0
    {
        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [UInt(1)] public bool Unknown0 { get; set; }
    }

    /// <summary>
    /// The kind of <see cref="Killer"/>, which picks its record: 0 a player, 1 a creature, 2 one the client has no name for.
    /// </summary>
    [UInt(2)] public byte KillerType { get; set; }

    /// <summary>
    /// The killer.
    /// </summary>
    [Union(nameof(KillerType))] public IServer05E7Union0 Killer { get; set; } = new Server05E7Union0Arm00();

    /// <summary>
    /// The kind of <see cref="Victim"/>, which picks its record: 0 a player, 1 a creature, 2 one the client has no name for.
    /// </summary>
    [UInt(2)] public byte VictimType { get; set; }

    /// <summary>
    /// The victim, in the same records as <see cref="Killer"/>.
    /// </summary>
    [Union(nameof(VictimType))] public IServer05E7Union0 Victim { get; set; } = new Server05E7Union0Arm00();

    /// <summary>
    /// The victim's team.
    /// </summary>
    [UInt(2)] public byte VictimTeam { get; set; }

    /// <summary>
    /// How the victim died.
    /// </summary>
    [UInt(3)] public PvpDeathReason DeathReason { get; set; }
}
