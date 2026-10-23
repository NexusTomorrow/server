using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// An item as a chat link describes it.
/// </summary>
[WireStruct]
public sealed partial class ChatItemRecord
{
    /// <summary>
    /// The item, by id.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }

    /// <summary>
    /// The item's type, an Item2 table id.
    /// </summary>
    [UInt(18)] public uint ItemId { get; set; }

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
