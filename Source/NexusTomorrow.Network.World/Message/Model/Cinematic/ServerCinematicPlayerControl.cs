using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicPlayerControl)]
[WireMessage(GameMessageOpcode.ServerCinematicPlayerControl, Direction.ServerToClient)]
public sealed partial class ServerCinematicPlayerControl : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(1)] public bool Allow { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(Allow);
    }
}
