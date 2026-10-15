using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSettlerHubUpdate)]
public class ServerPathSettlerHubUpdate : IWritable
{
    public uint PathSettlerHubId { get; set; }
    public uint AvenueEconomyValue { get; set; }
    public uint AvenueSecurityValue { get; set; }
    public uint AvenueQualityOfLifeValue { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSettlerHubId, 14);
        writer.Write(AvenueEconomyValue);
        writer.Write(AvenueSecurityValue);
        writer.Write(AvenueQualityOfLifeValue);
    }
}
