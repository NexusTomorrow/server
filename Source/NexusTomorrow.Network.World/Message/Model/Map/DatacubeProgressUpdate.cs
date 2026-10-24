using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Sets one datacube's progress; the client raises DatacubeUpdated.
/// </summary>
[WireMessage(GameMessageOpcode.Server00E1, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class DatacubeProgressUpdate
{
    /// <summary>
    /// The datacube's progress.
    /// </summary>
    public DatacubeProgress Progress { get; set; } = new();
}
