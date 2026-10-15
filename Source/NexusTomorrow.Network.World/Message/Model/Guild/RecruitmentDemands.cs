using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class RecruitmentDemands
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
}
