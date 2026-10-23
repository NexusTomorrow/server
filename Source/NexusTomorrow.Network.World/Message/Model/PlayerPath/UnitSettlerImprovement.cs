using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Sets which settler improvement a unit is.
/// </summary>
[WireMessage(GameMessageOpcode.Server08B6, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class UnitSettlerImprovement
{
    [WireStruct]
    public sealed partial class Server08B6Part0
    {
        [WideString] public string Unknown0 { get; set; } = "";
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    /// <summary>
    /// The unit, by unit id, that the rest of the message is about.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The settler improvement the unit is, which Unit:IsSettlerImprovement, IsSettlerReward, GetSettlerRewardName and
    /// GetSettlerImprovementInfo look up.
    /// </summary>
    [UInt(14)] public uint ImprovementId { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }
    [Array(CountBits = 32), WideString] public List<string> Unknown3 { get; set; } = [];
    [Array(CountBits = 32)] public List<Server08B6Part0> Unknown4 { get; set; } = [];
}
