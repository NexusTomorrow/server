using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Fixtures;

// These three declarations mirror real client layouts, so the facts check runs against the table
// before any real message is declared: 0x009A is 32, 16, 32, 1 bits; 0x0104 is 14, 32, 32 bits;
// 0x01C3 is 14, 64, a wide string, a 5-bit count of 37-bit elements, then 16 bits.

[WireMessage(GameMessageOpcode.ClientCastSpell, Direction.ClientToServer)]
public sealed partial class CastSpellFixture
{
    [UInt(32)] public uint ClientSpellCastUniqueId { get; set; }
    [UInt(16)] public ushort BagIndex { get; set; }
    [UInt(32)] public uint TargetUnitId { get; set; }
    [UInt(1)] public bool ButtonPressed { get; set; }
}

[WireMessage(GameMessageOpcode.ServerGalacticArchiveUpdate, Direction.ServerToClient)]
public sealed partial class ArchiveUpdateFixture
{
    [UInt(14)] public ushort ArticleId { get; set; }
    [UInt(32)] public uint UnlockedEntries { get; set; }
    [UInt(32)] public uint ViewedEntries { get; set; }
}

[WireMessage(GameMessageOpcode.ClientChat, Direction.ClientToServer)]
public sealed partial class ChatFixture
{
    public ChannelFixture Channel { get; set; } = new();
    [WideString] public string Message { get; set; } = "";
    [Array(CountBits = 5)] public List<ChatFormatFixture> Formats { get; } = [];
    [UInt(16)] public ushort ChatMessageId { get; set; }
}

[WireStruct]
public sealed partial class ChannelFixture
{
    [UInt(14)] public ushort ChannelType { get; set; }
    [UInt(64)] public ulong ChatId { get; set; }
}

[WireStruct]
public sealed partial class ChatFormatFixture
{
    [UInt(4)] public byte Type { get; set; }
    [UInt(16)] public ushort StartIndex { get; set; }
    [UInt(16)] public ushort StopIndex { get; set; }
    [Union(nameof(Type))] public IChatFormatFixture Model { get; set; } = new ChatFormatFlagFixture();
}

public interface IChatFormatFixture : IWireStruct
{
}

[WireStruct]
[UnionCase(0)]
public sealed partial class ChatFormatFlagFixture : IChatFormatFixture
{
    [UInt(1)] public bool Flag { get; set; }
}

[WireStruct]
[UnionCase(2)]
public sealed partial class ChatFormatItemFixture : IChatFormatFixture
{
    [UInt(18)] public uint ItemId { get; set; }
}
