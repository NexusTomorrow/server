using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathScientistUnitScanParameters)]
public class ServerPathScientistUnitScanParameters : IWritable
{
    public uint UnitId { get; set; }
    public ScanReward ScanRewardFlags { get; set; }
    public bool IsScannable { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(ScanRewardFlags, 32u);
        writer.Write(IsScannable);
    }
}
