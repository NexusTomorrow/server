using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

/// <summary>
/// Reports whether the player hides other players' combat log entries, each time the console variable cmbtlog.disableOtherPlayers
/// changes. The client applies the choice to its own combat log.
/// </summary>
[WireMessage(GameMessageOpcode.Client0248, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CombatLogOtherPlayersSet
{
    /// <summary>
    /// Whether cmbtlog.disableOtherPlayers is on. While it is, the client leaves other players' entries out of the combat log.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool DisableOtherPlayers { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It always sends 0.
    /// </summary>
    [VerifiedMember, UInt(31)] public uint Unknown1 { get; set; }
}
