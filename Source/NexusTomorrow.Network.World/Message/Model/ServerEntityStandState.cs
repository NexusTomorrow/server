using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityStandState)]
[WireMessage(GameMessageOpcode.ServerEntityStandState, Direction.ServerToClient)]
public sealed partial class ServerEntityStandState : IWritable
{
    [UInt(32)] public uint Guid { get; set; }
    [UInt(4)] public StandState StandState { get; set; }
    [UInt(14)] public uint EmoteId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(StandState, 4);
        writer.Write(EmoteId, 14);
    }
}
