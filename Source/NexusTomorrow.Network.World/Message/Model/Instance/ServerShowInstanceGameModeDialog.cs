using NexusTomorrow.Game.Static.Setting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.ServerShowInstanceGameModeDialog, Direction.ServerToClient)]
public sealed partial class ServerShowInstanceGameModeDialog
{
    [UInt(32)] public uint InstancePortalUnitId { get; set; }
    [UInt(2)] public WorldDifficulty Difficulty { get; set; }
    [UInt(11)] public InstanceParameters Parameters { get; set; }
    [UInt(2)] public WorldDifficulty ExistingDifficulty { get; set; }
    [UInt(8)] public byte MaxPrimeLevelWorld { get; set; }
    [UInt(8)] public byte MaxPrimeLevelGroup { get; set; }
    [UInt(8)] public byte ScalingLevel { get; set; }
    [UInt(1)] public bool ExistingScaling { get; set; }
    [UInt(8)] public byte ExistingPrimeLevel { get; set; }
    [UInt(1)] public bool IsRaid { get; set; }
    [UInt(1)] public bool PrimeAllowed { get; set; }
}
