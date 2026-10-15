using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathScientistExperimentationInvoke, Direction.ServerToClient)]
public sealed partial class ServerPathScientistExperimentationInvoke
{
    [UInt(32)] public uint UnitId { get; set; }
    [FixedArray(4), UInt(32)] public uint[] CurrentPattern { get; set; } = new uint[4]; // PathScientistExperimentationPatternDataId
}
