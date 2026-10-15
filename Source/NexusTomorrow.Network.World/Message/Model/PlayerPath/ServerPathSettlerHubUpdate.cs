using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSettlerHubUpdate, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerHubUpdate
{
    [UInt(14)] public uint PathSettlerHubId { get; set; }
    [UInt(32)] public uint AvenueEconomyValue { get; set; }
    [UInt(32)] public uint AvenueSecurityValue { get; set; }
    [UInt(32)] public uint AvenueQualityOfLifeValue { get; set; }
}
