using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Makes a matching attempt at a scientist experimentation.
/// </summary>
/// <remarks>
/// PathMission:AttemptScientistExperimentation sends it only when all four values match
/// <see cref="ScientistExperimentationStarted.Pattern"/> in place. On a mismatch the client scores the attempt itself, place by place (2
/// for a value in its place, 1 for a value elsewhere in the pattern, 0 for none), and sends nothing.
/// </remarks>
[WireMessage(GameMessageOpcode.Client066A, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ScientistExperimentationAttempt
{
    /// <summary>
    /// The four values tried, in the pattern's order.
    /// </summary>
    [FixedArray(4), UInt(32)] public uint[] Attempt { get; private set; } = new uint[4];
}
