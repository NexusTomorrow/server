using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerSetUnitPathType, Direction.ServerToClient)]
public sealed partial class ServerSetUnitPathType
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(3)] public Path Path { get; set; }
}
