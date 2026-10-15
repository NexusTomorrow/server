using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Triggers SetterHubReward lua event
[Message(GameMessageOpcode.ServerPathSettlerHubReward)]
public class ServerPathSettlerHubReward : IWritable
{
    public uint PathSettlerHubId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSettlerHubId, 14);
    }
}
