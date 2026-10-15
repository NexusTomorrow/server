using NexusTomorrow.Game.Static.Entity.Movement.Command.Mode;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicActorMode)]
[WireMessage(GameMessageOpcode.ServerCinematicActorMode, Direction.ServerToClient)]
public sealed partial class ServerCinematicActorMode : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public ModeType Mode { get; set; }
    [Pad(32)] public Pad ModeHigh { get; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(UnitId);
        writer.Write(Mode);
    }
}
