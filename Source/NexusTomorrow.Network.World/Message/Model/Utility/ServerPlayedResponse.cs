using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[Message(GameMessageOpcode.ServerPlayedResponse)]
[WireMessage(GameMessageOpcode.ServerPlayedResponse, Direction.ServerToClient)]
public sealed partial class ServerPlayedResponse : IWritable
{
    [UInt(64)] public ulong CreateTime { get; set; } // a Windows file time, as DateTime.ToFileTime gives it
    [UInt(32)] public uint TimePlayedSession { get; set; }
    [UInt(32)] public uint TimePlayedTotal { get; set; }
    [UInt(32)] public uint TimePlayedLevel { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreateTime);
        writer.Write(TimePlayedSession);
        writer.Write(TimePlayedTotal);
        writer.Write(TimePlayedLevel);
    }
}
