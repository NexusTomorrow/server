using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Fires whenever the player fails a Tracking mission.

[Message(GameMessageOpcode.ServerPathExplorerPowerMapFailed)]
public class ServerPathExplorerPowerMapFailed : IWritable
{
    public ushort PathMissionId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathMissionId, 14);
    }
}
