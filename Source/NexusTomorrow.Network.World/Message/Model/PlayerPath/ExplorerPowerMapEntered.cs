using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Marks the player inside an explorer power map.
/// </summary>
/// <remarks>
/// PathMission.IsExplorerPowerMapReady reads true until <see cref="ExplorerPowerMapExited"/>, and meanwhile the client sends
/// <see cref="ExplorerPowerMapProgress"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00F6, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ExplorerPowerMapEntered
{
    /// <summary>
    /// The power map, a PathExplorerPowerMap table id: the client finds the explorer mission whose object it is.
    /// </summary>
    [UInt(14)] public ushort PowerMapId { get; set; }
}
