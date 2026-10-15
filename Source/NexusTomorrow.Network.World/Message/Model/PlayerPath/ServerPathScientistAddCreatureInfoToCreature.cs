using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathScientistAddCreatureInfoToCreature, Direction.ServerToClient)]
public sealed partial class ServerPathScientistAddCreatureInfoToCreature
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(14)] public uint PathScientistCreatureInfoId { get; set; }
}
