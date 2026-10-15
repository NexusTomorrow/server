using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSettlerBuildStatus)]
[WireMessage(GameMessageOpcode.ServerPathSettlerBuildStatus, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerBuildStatus : IWritable
{
    [UInt(14)] public ushort PathSettlerHubId { get; set; }
    public SettlerImprovementGroupStatus Status { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSettlerHubId, 14);
        Status.Write(writer);
    }
}
