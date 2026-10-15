using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicCamera)]
[WireMessage(GameMessageOpcode.ServerCinematicCamera, Direction.ServerToClient)]
public sealed partial class ServerCinematicCamera : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public CameraAddFlags Flags { get; set; }
    [UInt(32)] public uint EndTransition { get; set; } // specifies type of transition
    [UInt(16)] public ushort TranDurationStart { get; set; }
    [UInt(16)] public ushort TranDurationMid { get; set; }
    [UInt(16)] public ushort TranDurationEnd { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(Flags, 32u);
        writer.Write(EndTransition);
        writer.Write(TranDurationStart);
        writer.Write(TranDurationMid);
        writer.Write(TranDurationEnd);
    }
}
