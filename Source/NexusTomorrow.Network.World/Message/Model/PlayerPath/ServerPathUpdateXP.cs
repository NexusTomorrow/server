using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathUpdateXP)]
[WireMessage(GameMessageOpcode.ServerPathUpdateXP, Direction.ServerToClient)]
public sealed partial class ServerPathUpdateXP : IWritable
{
    [UInt(32)] public uint TotalXP { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TotalXP);
    }
}
