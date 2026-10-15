using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ClientPathExplorerPowerMapProgress, Direction.ClientToServer)]
public sealed partial class ClientPathExplorerPowerMapProgress
{
    [UInt(14)] public uint PathMissionId { get; private set; }
}
