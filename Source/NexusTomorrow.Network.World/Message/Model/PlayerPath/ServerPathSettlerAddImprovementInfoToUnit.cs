using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSettlerAddImprovementInfoToUnit, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerAddImprovementInfoToUnit
{
    [WireStruct]
    public sealed partial class ImprovementInfo
    {
        [WideString] public string Name { get; set; }
        [UInt(32)] public uint Tier { get; set; }
    }

    [UInt(32)] public uint UnitId { get; set; }
    [UInt(14)] public uint PathSettlerImprovementGroupId { get; set; }
    [UInt(32)] public uint RemainingTime { get; set; }
    [Array(CountBits = 32), WideString] public List<string> Owners { get; set; } = [];
    [Array(CountBits = 32)] public List<ImprovementInfo> Improvements { get; set; } = [];
}
