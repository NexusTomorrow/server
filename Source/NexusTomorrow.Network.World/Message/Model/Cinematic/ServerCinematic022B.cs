using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematic022B)]
[WireMessage(GameMessageOpcode.ServerCinematic022B, Direction.ServerToClient)]
public sealed partial class ServerCinematic022B : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(Unknown1);
    }
}
