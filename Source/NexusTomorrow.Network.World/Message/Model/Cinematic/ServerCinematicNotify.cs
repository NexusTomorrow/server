using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicNotify)]
[WireMessage(GameMessageOpcode.ServerCinematicNotify, Direction.ServerToClient)]
public sealed partial class ServerCinematicNotify : IWritable
{
    [UInt(16)] public CinematicFlags Flags { get; set; }
    [UInt(16)] public CancelType CancelMode { get; set; }
    [UInt(32)] public uint Delay { get; set; } // for scheduling end
    [UInt(14)] public ushort CinematicId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Flags, 16u);
        writer.Write(CancelMode, 16u);
        writer.Write(Delay);
        writer.Write(CinematicId, 14u);
    }
}
