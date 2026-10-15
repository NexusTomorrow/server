using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerResurrectionUpdate)]
[WireMessage(GameMessageOpcode.ServerResurrectionUpdate, Direction.ServerToClient)]
public sealed partial class ServerResurrectionUpdate : IWritable
{
    [UInt(8)] public ResurrectionType ShowRezFlags { get; set; } // 8
    [UInt(1)] public bool HasCasterRezRequest { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ShowRezFlags, 8u);
        writer.Write(HasCasterRezRequest);
    }
}
