using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerSetUnitPathType)]
[WireMessage(GameMessageOpcode.ServerSetUnitPathType, Direction.ServerToClient)]
public sealed partial class ServerSetUnitPathType : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(3)] public Path Path { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(Path, 3);
    }
}
