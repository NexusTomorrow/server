using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSoldierHoldoutEnd)]
[WireMessage(GameMessageOpcode.ServerPathSoldierHoldoutEnd, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierHoldoutEnd : IWritable
{
    [UInt(14)] public ushort PathSoldierEventId { get; set; }
    [UInt(32)] public PlayerPathSoldierResult Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSoldierEventId, 14);
        writer.Write(Reason, 32u);
    }
}
