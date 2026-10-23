using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Reports an explorer mission whose location the player stands in.
/// </summary>
/// <remarks>
/// The client sends it for a PathMission row of type 3, 15 or 27 while the player stands in the mission's world location.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00F3, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class ExplorerProgressReport
{
    /// <summary>
    /// The mission, a PathMission table id.
    /// </summary>
    [UInt(15)] public uint MissionId { get; private set; }

    [UInt(32)] public uint Unknown1 { get; private set; }
}
