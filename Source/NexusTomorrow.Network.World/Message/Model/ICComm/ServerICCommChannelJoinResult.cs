using NexusTomorrow.Game.Static.ICComm;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

// Presumably sent if a client is not allowed to join a channel but none seen in sniffs
[Message(GameMessageOpcode.ServerICCommChannelJoinResult)]
[WireMessage(GameMessageOpcode.ServerICCommChannelJoinResult, Direction.ServerToClient)]
public sealed partial class ServerICCommChannelJoinResult : IWritable
{
    [UInt(3)] public ICCommChannelType Type { get; set; }
    [UInt(4)] public ICCommJoinResult Result { get; set; }
    [WideString] public string ChannelName { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Type, 3u);
        writer.Write(Result, 4u);
        writer.WriteStringWide(ChannelName);
    }
}
