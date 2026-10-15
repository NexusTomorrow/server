using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSettlerHubUpdate)]
[WireMessage(GameMessageOpcode.ServerPathSettlerHubUpdate, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerHubUpdate : IWritable
{
    [UInt(14)] public uint PathSettlerHubId { get; set; }
    [UInt(32)] public uint AvenueEconomyValue { get; set; }
    [UInt(32)] public uint AvenueSecurityValue { get; set; }
    [UInt(32)] public uint AvenueQualityOfLifeValue { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSettlerHubId, 14);
        writer.Write(AvenueEconomyValue);
        writer.Write(AvenueSecurityValue);
        writer.Write(AvenueQualityOfLifeValue);
    }
}
