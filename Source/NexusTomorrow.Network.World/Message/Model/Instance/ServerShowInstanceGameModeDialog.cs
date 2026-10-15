using NexusTomorrow.Game.Static.Setting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[Message(GameMessageOpcode.ServerShowInstanceGameModeDialog)]
[WireMessage(GameMessageOpcode.ServerShowInstanceGameModeDialog, Direction.ServerToClient)]
public sealed partial class ServerShowInstanceGameModeDialog : IWritable
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

    public void Write(GamePacketWriter writer)
    {
        writer.Write(InstancePortalUnitId);
        writer.Write(Difficulty, 2u);
        writer.Write(Parameters, 11u);
        writer.Write(ExistingDifficulty, 2u);
        writer.Write(MaxPrimeLevelWorld);
        writer.Write(MaxPrimeLevelGroup);
        writer.Write(ScalingLevel);
        writer.Write(ExistingScaling);
        writer.Write(ExistingPrimeLevel);
        writer.Write(IsRaid);
        writer.Write(PrimeAllowed);
    }
}
