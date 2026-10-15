using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Triggers SetterHubReward lua event
[Message(GameMessageOpcode.ServerPathSettlerHubReward)]
[WireMessage(GameMessageOpcode.ServerPathSettlerHubReward, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerHubReward : IWritable
{
    [UInt(14)] public uint PathSettlerHubId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSettlerHubId, 14);
    }
}
