using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicPlatformDetachActor)]
[WireMessage(GameMessageOpcode.ServerCinematicPlatformDetachActor, Direction.ServerToClient)]
public sealed partial class ServerCinematicPlatformDetachActor : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint UnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(UnitId);
    }
}
