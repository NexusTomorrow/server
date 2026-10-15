using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSoldierHoldoutDeath)]
[WireMessage(GameMessageOpcode.ServerPathSoldierHoldoutDeath, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierHoldoutDeath : IWritable
{
    [UInt(14)] public ushort PathSoldierEventId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSoldierEventId, 14);
    }
}
