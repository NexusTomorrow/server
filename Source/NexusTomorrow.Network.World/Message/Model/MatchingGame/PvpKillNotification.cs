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
[Verified(Rung.Purpose)]
public sealed partial class PvpKillNotification
{
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
        /// The player, by realm-scoped character id.
        /// </summary>
        public RealmScopedId PlayerId { get; set; } = new();

        /// <summary>
        /// The player's class; the client reports the killer's.
        /// </summary>
        [UInt(5)] public byte Class { get; set; }
    }

    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Server05E7Union0Arm01 : IServer05E7Union0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(18)] public uint Unknown1 { get; set; }
    }

    [WireStruct]
    [UnionCase(2)]
    public sealed partial class Server05E7Union0Arm02 : IServer05E7Union0
    {
        [UInt(1)] public bool Unknown0 { get; set; }
    }

    /// <summary>
    /// The kind of <see cref="Killer"/>, which picks its record.
    /// </summary>
    [UInt(2)] public byte KillerType { get; set; }

    /// <summary>
    /// The killer.
    /// </summary>
    [Union(nameof(KillerType))] public IServer05E7Union0 Killer { get; set; } = new Server05E7Union0Arm00();

    /// <summary>
    /// The kind of <see cref="Victim"/>, which picks its record.
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
