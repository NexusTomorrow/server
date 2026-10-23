using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Clears an explorer power map mission's inside mark, which PathMission:IsExplorerPowerMapReady reads.
/// </summary>
[WireMessage(GameMessageOpcode.Server00F7, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ExplorerPowerMapExited
{
    /// <summary>
    /// The power map, a PathExplorerPowerMap table id: the client finds the explorer mission whose object it is.
    /// </summary>
    [UInt(14)] public ushort PowerMapId { get; set; }
}
