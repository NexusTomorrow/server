using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Sets one datacube volume's progress; the client raises DatacubeUpdated.
/// </summary>
[WireMessage(GameMessageOpcode.Server00E2, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class DatacubeVolumeProgressUpdate
{
    /// <summary>
    /// The volume's progress.
    /// </summary>
    public DatacubeVolumeProgress Progress { get; set; } = new();
}
