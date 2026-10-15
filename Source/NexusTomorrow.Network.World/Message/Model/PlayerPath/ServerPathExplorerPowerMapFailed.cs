using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Fires whenever the player fails a Tracking mission.

[Message(GameMessageOpcode.ServerPathExplorerPowerMapFailed)]
[WireMessage(GameMessageOpcode.ServerPathExplorerPowerMapFailed, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerPowerMapFailed : IWritable
{
    [UInt(14)] public ushort PathMissionId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathMissionId, 14);
    }
}
