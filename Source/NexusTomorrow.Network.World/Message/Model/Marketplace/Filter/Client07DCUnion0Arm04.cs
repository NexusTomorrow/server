using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

/// <summary>
/// The <see cref="AuctionFilterType.RuneSlot"/> filter: a microchip type and a minimum.
/// </summary>
[WireStruct]
[UnionCase(4)]
public sealed partial class Client07DCUnion0Arm04 : IClient07DCUnion0
{
    /// <summary>
    /// The microchip type, up to 0x11.
    /// </summary>
    [UInt(5)] public byte MicrochipType { get; private set; }

    /// <summary>
    /// At most 8; the client sends 1 when none is given.
    /// </summary>
    [UInt(32)] public uint Min { get; private set; }
}
