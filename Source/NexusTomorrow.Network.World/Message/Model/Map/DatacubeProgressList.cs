using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Sets all of the player's datacube progress: the client replaces all its datacube progress with these lists, then raises
/// DatacubeUpdated.
/// </summary>
[WireMessage(GameMessageOpcode.Server00E0, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class DatacubeProgressList
{
    /// <summary>
    /// The progress of each datacube.
    /// </summary>
    [Array(CountBits = 32)] public List<DatacubeProgress> Datacubes { get; set; } = [];

    /// <summary>
    /// The progress of each datacube volume.
    /// </summary>
    [Array(CountBits = 32)] public List<DatacubeVolumeProgress> Volumes { get; set; } = [];
}
