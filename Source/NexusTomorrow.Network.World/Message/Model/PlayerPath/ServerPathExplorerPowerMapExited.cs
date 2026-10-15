using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath
{
    // Fires whenever the player leaves a location where a power map mission can be started.

    [Message(GameMessageOpcode.ServerPathExplorerPowerMapExited)]
    public class ServerPathExplorerPowerMapExited : IWritable
    {
        public ushort PathMissionId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(PathMissionId, 14);
        }
    }
}
