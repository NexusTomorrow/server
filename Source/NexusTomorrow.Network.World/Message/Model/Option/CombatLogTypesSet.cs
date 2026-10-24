using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

/// <summary>
/// Reports which combat log entry types the player hides, each time one of the console variables for a type changes. The client applies
/// the mask to its own combat log.
/// </summary>
[WireMessage(GameMessageOpcode.Client0249, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CombatLogTypesSet
{
    /// <summary>
    /// The hidden entry types: bits 0 to 13, as <see cref="CombatLogType"/> names them, each set while its console variable is on. The
    /// client gives bits 14 to 31 no meaning, and always sends them as 0.
    /// </summary>
    [VerifiedMember, UInt(32)] public CombatLogType DisabledTypes { get; set; }
}
