using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerSetUnitPathType)]
public class ServerSetUnitPathType : IWritable
{
    public uint UnitId { get; set; }
    public Path Path { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(Path, 3);
    }
}
