using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Message01B8, Direction.Both)]
public sealed partial class ChatLinkRecord
{
    public interface IMessage01B8Union0 : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(0)]
    public sealed partial class Message01B8Union0Arm00 : IMessage01B8Union0
    {
        [UInt(1)] public bool Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Message01B8Union0Arm01 : IMessage01B8Union0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(2)]
    public sealed partial class Message01B8Union0Arm02 : IMessage01B8Union0
    {
        [UInt(1)] public bool Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(3)]
    public sealed partial class Message01B8Union0Arm03 : IMessage01B8Union0
    {
        [UInt(1)] public bool Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(4)]
    public sealed partial class Message01B8Union0Arm04 : IMessage01B8Union0
    {
        [UInt(18)] public uint Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(5)]
    public sealed partial class Message01B8Union0Arm05 : IMessage01B8Union0
    {
        [UInt(15)] public ushort Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(6)]
    public sealed partial class Message01B8Union0Arm06 : IMessage01B8Union0
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(7)]
    public sealed partial class Message01B8Union0Arm07 : IMessage01B8Union0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(8)]
    public sealed partial class Message01B8Union0Arm08 : IMessage01B8Union0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        [UInt(18)] public uint Unknown1 { get; set; }
        [UInt(64)] public ulong Unknown2 { get; set; }
        [UInt(64)] public ulong Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }
        [UInt(64)] public ulong Unknown5 { get; set; }
        [UInt(32)] public uint Unknown6 { get; set; }
        [UInt(32)] public uint Unknown7 { get; set; }
        [UInt(8)] public byte Unknown8 { get; set; }
        [UInt(18)] public uint Unknown9 { get; set; }
        [Array(CountBits = 3), UInt(32)] public List<uint> Unknown10 { get; } = [];
        [Array(CountBits = 4), UInt(32)] public List<uint> Unknown11 { get; } = [];
    }

    [WireStruct]
    [UnionCase(9)]
    public sealed partial class Message01B8Union0Arm09 : IMessage01B8Union0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
    }

    [WireStruct]
    [UnionCase(10)]
    public sealed partial class Message01B8Union0Arm0A : IMessage01B8Union0
    {
        [UInt(14)] public ushort Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
    }

    [WireStruct]
    [UnionCase(11)]
    public sealed partial class Message01B8Union0Arm0B : IMessage01B8Union0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
    }

    [UInt(4)] public byte Unknown0 { get; set; }
    [UInt(16)] public ushort Unknown1 { get; set; }
    [UInt(16)] public ushort Unknown2 { get; set; }
    [Union(nameof(Unknown0))] public IMessage01B8Union0 Unknown3 { get; set; } = new Message01B8Union0Arm00();
}
