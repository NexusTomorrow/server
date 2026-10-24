using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

/// <summary>
/// Marks the player's instance busy or not busy: the client raises InstanceSetBusy with <see cref="Busy"/>.
/// </summary>
/// <remarks>
/// While busy, the client withholds its overhead anchor and quest callout markers on units, which it otherwise shows only
/// under the world's resurrection-at-caster option.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0555, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class InstanceBusy
{
    /// <summary>
    /// Whether the instance is busy.
    /// </summary>
    [UInt(1)] public bool Busy { get; set; }
}
