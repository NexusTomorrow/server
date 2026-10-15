using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class RecruitmentDemands : IWritable
{
    [UInt(8)] public RecruitmendDemand WarriorAssault { get; set; }
    [UInt(8)] public RecruitmendDemand WarriorSupport { get; set; }
    [UInt(8)] public RecruitmendDemand EngineerAssault { get; set; }
    [UInt(8)] public RecruitmendDemand EngineerSupport { get; set; }
    [UInt(8)] public RecruitmendDemand EsperAssault { get; set; }
    [UInt(8)] public RecruitmendDemand EsperSupport { get; set; }
    [UInt(8)] public RecruitmendDemand MedicAssault { get; set; }
    [UInt(8)] public RecruitmendDemand MedicSupport { get; set; }
    [UInt(8)] public RecruitmendDemand StalkedAssault { get; set; }
    [UInt(8)] public RecruitmendDemand StalkerSupport { get; set; }
    [UInt(8)] public RecruitmendDemand SpellslingerAssault { get; set; }
    [UInt(8)] public RecruitmendDemand SpellslingerSupport { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(WarriorAssault, 8u);
        writer.Write(WarriorSupport, 8u);
        writer.Write(EngineerAssault, 8u);
        writer.Write(EngineerSupport, 8u);
        writer.Write(EsperAssault, 8u);
        writer.Write(EsperSupport, 8u);
        writer.Write(MedicAssault, 8u);
        writer.Write(MedicSupport, 8u);
        writer.Write(StalkedAssault, 8u);
        writer.Write(StalkerSupport, 8u);
        writer.Write(SpellslingerAssault, 8u);
        writer.Write(SpellslingerSupport, 8u);
    }
}
