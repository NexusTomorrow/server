using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Makes an attempt at a scientist experimentation.
/// </summary>
/// <remarks>
/// PathMission.AttemptScientistExperimentation sends it. The client scores the attempt against
/// <see cref="ScientistExperimentationStarted.Pattern"/> place by place: 2 for a value in its place, 1 for a value elsewhere in the
/// pattern, 0 for none.
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
