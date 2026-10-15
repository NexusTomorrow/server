using NexusTomorrow.Game.Static.Cinematic;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicScaleTransition)]
[WireMessage(GameMessageOpcode.ServerCinematicScaleTransition, Direction.ServerToClient)]
public sealed partial class ServerCinematicTransitionDurationSet : IWritable
{
    [UInt(32)] public ScaleTransitionType Type { get; set; }
    [UInt(16)] public ushort DurationStart { get; set; }
    [UInt(16)] public ushort DurationMid { get; set; }
    [UInt(16)] public ushort DurationEnd { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Type, 32u);
        writer.Write(DurationStart);
        writer.Write(DurationMid);
        writer.Write(DurationEnd);
    }
}
