using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSoldierBuildCredits)]
[WireMessage(GameMessageOpcode.ServerPathSoldierBuildCredits, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierBuildCredits : IWritable
{
    [UInt(14)] public ushort PathSoldierEventId { get; set; }
    [UInt(32)] public uint BuildCredits { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSoldierEventId, 14);
        writer.Write(BuildCredits);
    }
}
