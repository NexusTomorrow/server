using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Sets an explorer power map mission's inside mark, which PathMission:IsExplorerPowerMapReady reads.
/// </summary>
/// <remarks>
/// While the mark is set the client sends <see cref="ExplorerPowerMapProgress"/>; <see cref="ExplorerPowerMapExited"/> clears it.
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
