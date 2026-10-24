using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0262Part10
{
    [WireStruct]
    public sealed partial class Server0262Part11
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(32)] public uint Unknown5 { get; set; }
        [UInt(32)] public uint Unknown6 { get; set; }
        [UInt(3)] public byte Unknown7 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }

    /// <summary>
    /// The amount dealt or healed. The combat log reports it as nHealAmount for a heal and as nDamageAmount for damage to shields.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Amount { get; set; }

    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }

    /// <summary>
    /// Whether the effect killed the target, which the combat log reports as bTargetKilled.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool TargetKilled { get; set; }

    /// <summary>
    /// How the effect landed, which the combat log reports as eCombatResult. The client reports no damage or heal entry for the effect
    /// when it is 0.
    /// </summary>
    [VerifiedMember, UInt(4)] public CombatResult CombatResult { get; set; }

    [UInt(3)] public byte Unknown9 { get; set; }

    /// <summary>
    /// The effect's further hits. The client reads only the first.
    /// </summary>
    [VerifiedMember, Array(CountBits = 8)] public List<Server0262Part11> MultiHits { get; set; } = [];
}
