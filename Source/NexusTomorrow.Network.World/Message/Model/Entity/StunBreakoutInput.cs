using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Reports the direction keys pressed and held while the player breaks out of a stun.
/// </summary>
/// <remarks>
/// The client sends it when the keys held change or a key was pressed, and counts the input toward breaking out when it
/// matches the direction <see cref="StunBreakoutDirection"/> gave.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00C3, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class StunBreakoutInput
{
    /// <summary>
    /// The direction keys pressed since the last report.
    /// </summary>
    [UInt(8)] public CCStateStunVictimGameplay Pressed { get; private set; }

    /// <summary>
    /// The direction keys held now.
    /// </summary>
    [UInt(8)] public CCStateStunVictimGameplay Held { get; private set; }
}
