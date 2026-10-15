using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Fires whenever the player enters an area where a Tracking mission can be started

[Message(GameMessageOpcode.ServerPathExplorerPowerMapEntered)]
[WireMessage(GameMessageOpcode.ServerPathExplorerPowerMapEntered, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerPowerMapEntered : IWritable
{
    [UInt(14)] public ushort PathMissionId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathMissionId, 14);
    }
}
