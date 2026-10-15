using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath
{
    [Message(GameMessageOpcode.ServerPathSettlerBuildStatusList)]
    public class ServerPathSettlerBuildStatusList : IWritable
    {
        public ushort PathSettlerHubId { get; set; }
        public List<SettlerImprovementGroupStatus> ImprovementGroupStatuses { get; set; } = [];

        public void Write(GamePacketWriter writer)
        {
            writer.Write(PathSettlerHubId, 14);
            writer.Write(ImprovementGroupStatuses.Count);
            ImprovementGroupStatuses.ForEach(status => status.Write(writer));
        }
    }
}
