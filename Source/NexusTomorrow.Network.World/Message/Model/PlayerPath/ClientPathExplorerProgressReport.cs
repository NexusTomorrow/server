using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ClientPathExplorerProgressReport, Direction.ClientToServer)]
public sealed partial class ClientPathExplorerProgressReport
{
    [UInt(15)] public uint PathMissionId { get; private set; }
    [UInt(32)] public uint ExplorerNodeIndex { get; private set; } // Reports when player unit has reached radius threshold around WorldLocation.
}
