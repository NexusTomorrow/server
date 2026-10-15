using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupActionResult)]
[WireMessage(GameMessageOpcode.ServerGroupActionResult, Direction.ServerToClient)]
public sealed partial class ServerGroupActionResult : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }
    public Identity Identity { get; set; }
    [UInt(32)] public GroupActionResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        Identity.Write(writer);
        writer.Write(Result, 32);
    }
}
