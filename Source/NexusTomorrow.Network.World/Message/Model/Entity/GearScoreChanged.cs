using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets the player's gear score.
/// </summary>
/// <remarks>
/// The client raises UpdateGearScore.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01A6, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GearScoreChanged
{
    /// <summary>
    /// The gear score.
    /// </summary>
    [Float32] public float GearScore { get; set; }
}
