using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
public sealed partial class CombatLogAction
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(18)] public uint Unknown2 { get; set; }
    [UInt(4)] public byte Unknown3 { get; set; }
}
