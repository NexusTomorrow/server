using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Entity.Movement.Command.Mode;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicActorAdd, Direction.ServerToClient)]
public sealed partial class ServerCinematicActorAdd
{
    [UInt(32)] public uint Delay { get; set; }
    [UInt(16)] public EntityCreateFlag CreateFlags { get; set; }
    [UInt(16)] public ushort TextureLevelOfDetailBias { get; set; } // outside chance this is a signed value but sniffs do not show it used that way
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint Creature2Id { get; set; }
    [UInt(32)] public ModeType MovementMode { get; set; }
    public Position Position { get; set; }
    [UInt(64)] public ulong ActivePropId { get; set; }
    [UInt(32)] public uint WorldSocketId { get; set; }
}
