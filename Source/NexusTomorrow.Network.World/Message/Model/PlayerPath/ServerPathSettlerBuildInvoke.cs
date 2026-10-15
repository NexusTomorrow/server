using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath
{
    // Fires whenever a settler interacts with a settler depot.
    [Message(GameMessageOpcode.ServerPathSettlerBuildInvoke)]
    public class ServerPathSettlerBuildInvoke : IWritable
    {
        public uint SettlerHubUnitId { get; set; }
        public List<uint> PathSettlerImprovementGroupIds { get; set; } = [];

        public void Write(GamePacketWriter writer)
        {
            writer.Write(SettlerHubUnitId);
            writer.Write(PathSettlerImprovementGroupIds.Count);
            PathSettlerImprovementGroupIds.ForEach(id => writer.Write(id)); 
        }
    }
}
