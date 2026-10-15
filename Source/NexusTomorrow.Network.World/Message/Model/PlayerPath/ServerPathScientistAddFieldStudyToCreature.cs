using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathScientistAddFieldStudyToCreature, Direction.ServerToClient)]
public sealed partial class ServerPathScientistAddFieldStudyToCreature
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(14)] public uint PathScientistFieldStudyId { get; set; }
}
