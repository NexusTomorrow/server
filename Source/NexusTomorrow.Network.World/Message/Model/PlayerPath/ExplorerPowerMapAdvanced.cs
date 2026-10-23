using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Advances an explorer power map mission by one step.
/// </summary>
/// <remarks>
/// The client adds one to the mission's step count and raises PlayerPathMissionAdvanced.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00F4, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ExplorerPowerMapAdvanced
{
    /// <summary>
    /// The power map, a PathExplorerPowerMap table id: the client finds the explorer mission whose object it is.
    /// </summary>
    [UInt(14)] public ushort PowerMapId { get; set; }
}
