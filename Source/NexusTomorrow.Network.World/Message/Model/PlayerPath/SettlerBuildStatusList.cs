using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server066E, Direction.ServerToClient)]
public sealed partial class SettlerBuildStatusList
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<SettlerImprovementStatus> Unknown1 { get; set; } = [];
}
