using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSoldierTriggerOrdersTextOnUnit)]
[WireMessage(GameMessageOpcode.ServerPathSoldierTriggerOrdersTextOnUnit, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierTriggerOrdersTextOnUnit : IWritable
{
    [UInt(15)] public ushort PathMissionId { get; set; }
    [UInt(32)] public uint UnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathMissionId, 15);
        writer.Write(UnitId);
    }
}
