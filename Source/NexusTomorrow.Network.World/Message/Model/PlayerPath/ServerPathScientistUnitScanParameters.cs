using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathScientistUnitScanParameters)]
[WireMessage(GameMessageOpcode.ServerPathScientistUnitScanParameters, Direction.ServerToClient)]
public sealed partial class ServerPathScientistUnitScanParameters : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public ScanReward ScanRewardFlags { get; set; }
    [UInt(1)] public bool IsScannable { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(ScanRewardFlags, 32u);
        writer.Write(IsScannable);
    }
}
