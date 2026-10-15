using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
public sealed partial class SpellInit : IWritable
{
    [UInt(32)] public uint CasterUnitId { get; set; }
    [UInt(32)] public uint TargetUnitId { get; set; }
    [UInt(32)] public uint ServerSpellCastUniqueId { get; set; }
    [UInt(18)] public uint Spell4Id { get; set; }
    [UInt(1)] public bool BIgnoreCooldown { get; set; }
    [Array(CountBits = 32)] public List<TargetInfo> TargetInfoData { get; set; } = [];
    [Array(CountBits = 8)] public List<InitialPosition> InitialPositionData { get; set; } = [];
    [Array(CountBits = 8)] public List<TelegraphPosition> TelegraphPositionData { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CasterUnitId);
        writer.Write(TargetUnitId);
        writer.Write(ServerSpellCastUniqueId);
        writer.Write(Spell4Id, 18u);
        writer.Write(BIgnoreCooldown);

        writer.Write(TargetInfoData.Count, 32u);
        TargetInfoData.ForEach(u => u.Write(writer));

        writer.Write(InitialPositionData.Count, 8u);
        InitialPositionData.ForEach(u => u.Write(writer));

        writer.Write(TelegraphPositionData.Count, 8u);
        TelegraphPositionData.ForEach(u => u.Write(writer));
    }
}
