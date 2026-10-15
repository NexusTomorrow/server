using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Entity.Movement.Command.Mode;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicActorAdd)]
public class ServerCinematicActorAdd : IWritable
{
    public uint Delay { get; set; }
    public EntityCreateFlag CreateFlags { get; set; }
    public ushort TextureLevelOfDetailBias { get; set; } // outside chance this is a signed value but sniffs do not show it used that way
    public uint UnitId { get; set; }
    public uint Creature2Id { get; set; }
    public ModeType MovementMode { get; set; }
    public Position Position { get; set; }
    public ulong ActivePropId { get; set; }
    public uint WorldSocketId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(CreateFlags, 16u);
        writer.Write(TextureLevelOfDetailBias);
        writer.Write(UnitId);
        writer.Write(Creature2Id);
        writer.Write(MovementMode, 32u);
        Position.Write(writer);
        writer.Write(ActivePropId);
        writer.Write(WorldSocketId);
    }
}
