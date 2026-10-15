using NexusTomorrow.Game.Static.Entity.Movement.Command.Mode;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicActorMode)]
public class ServerCinematicActorMode : IWritable
{
    public uint Delay { get; set; }
    public uint UnitId { get; set; }
    public ModeType Mode { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(UnitId);
        writer.Write(Mode);
    }
}
