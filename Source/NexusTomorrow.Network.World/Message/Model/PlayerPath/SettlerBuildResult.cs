using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Reports the result of a settler build.
/// </summary>
/// <remarks>
/// The client takes it only while the player is using a settler hub. It raises SettlerBuildResult with <see cref="Unknown0"/> and, when
/// <see cref="Unknown1"/> is not 0, the name of the improvement <see cref="ImprovementId"/> finds.
/// </remarks>
[WireMessage(GameMessageOpcode.Server066C, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SettlerBuildResult
{
    [UInt(32)] public uint Unknown0 { get; set; }

    [UInt(15)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The improvement whose name the result shows, by id.
    /// </summary>
    [UInt(14)] public uint ImprovementId { get; set; }
}
