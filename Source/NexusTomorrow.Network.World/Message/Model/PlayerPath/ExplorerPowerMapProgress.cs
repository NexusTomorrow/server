using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Reports the explorer power map the player is inside.
/// </summary>
/// <remarks>
/// The client sends it while the mission's inside mark is set (<see cref="ExplorerPowerMapEntered"/>).
/// </remarks>
[WireMessage(GameMessageOpcode.Client00F9, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ExplorerPowerMapProgress
{
    /// <summary>
    /// The power map, a PathExplorerPowerMap table id: the object of the explorer mission.
    /// </summary>
    [UInt(14)] public uint PowerMapId { get; private set; }
}
