using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ServerForceKick)]
[WireMessage(GameMessageOpcode.ServerForceKick, Direction.ServerToClient)]
public sealed partial class ServerForceKick : IWritable
{
    [UInt(5)] public ForceKickReason Reason { get; set; } = ForceKickReason.WorldDisconnect;

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Reason, 5u);
    }
}
