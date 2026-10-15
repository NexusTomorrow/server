using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(ChatFormatType.ItemFull)]
public sealed partial class ChatFormatItemFull : IChatFormatModel
{
    [NotOnWire] public ChatFormatType Type => ChatFormatType.ItemFull;
    [UInt(64)] public ulong ItemGuid { get; set; }
    [UInt(18)] public uint Item2Id { get; set; }
    [UInt(64)] public ulong MakerCharacterId { get; set; }
    [UInt(64)] public ulong CircuitData { get; set; }
    [UInt(32)] public uint RuneData { get; set; }
    [UInt(64)] public ulong ThresholdData { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint WorldRequirements { get; set; }
    [UInt(8)] public byte Unknown2 { get; set; }
    [UInt(18)] public uint WorldRequirementItem2Id { get; set; }
    [Array(CountBits = 3), Float32] public List<float> ChargeAmounts { get; set; } = [];
    [Array(CountBits = 4), UInt(32)] public List<uint> RuneSlotItem2Ids { get; set; } = [];
}
