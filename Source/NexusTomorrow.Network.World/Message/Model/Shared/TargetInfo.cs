using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

public class TargetInfo : IWritable // same used for 0x0818
{
    public uint UnitId { get; set; }
    public byte Ndx { get; set; }
    public byte TargetFlags { get; set; }
    public ushort InstanceCount { get; set; }
    public CombatResult CombatResult { get; set; }

    public List<EffectInfo> EffectInfoData { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(Ndx);
        writer.Write(TargetFlags);
        writer.Write(InstanceCount);
        writer.Write(CombatResult, 4u);

        writer.Write(EffectInfoData.Count, 8u);
        EffectInfoData.ForEach(u => u.Write(writer));
    }

}
