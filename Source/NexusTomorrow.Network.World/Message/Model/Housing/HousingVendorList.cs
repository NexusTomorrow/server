using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Lists what a residence's vendor offers.
/// </summary>
[WireMessage(GameMessageOpcode.Server0508, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HousingVendorList
{
    /// <summary>
    /// One item offered.
    /// </summary>
    [WireStruct]
    public sealed partial class VendorItem
    {
        [UInt(64)] public ulong Unknown0 { get; set; }

        /// <summary>
        /// The item, a HousingPlugItem table id.
        /// </summary>
        [UInt(32)] public uint PlugItemId { get; set; }

        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
    }

    /// <summary>
    /// The items offered.
    /// </summary>
    [Array(CountBits = 32)] public List<VendorItem> Items { get; set; } = [];

    [UInt(2)] public byte Unknown1 { get; set; }
}
