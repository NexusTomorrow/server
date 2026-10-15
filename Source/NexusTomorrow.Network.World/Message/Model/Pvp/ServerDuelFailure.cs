using NexusTomorrow.Game.Static.Pvp;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[Message(GameMessageOpcode.ServerDuelFailure)]
[WireMessage(GameMessageOpcode.ServerDuelFailure, Direction.ServerToClient)]
public sealed partial class ServerDuelFailure : IWritable
{
    [UInt(5)] public DuelFailureReason Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Reason, 5u);
    }
}
