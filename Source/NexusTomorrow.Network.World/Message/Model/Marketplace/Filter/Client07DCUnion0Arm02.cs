using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

/// <summary>
/// The <see cref="AuctionFilterType.Level"/> filter: a range of item levels.
/// </summary>
[WireStruct]
[UnionCase(2)]
public sealed partial class Client07DCUnion0Arm02 : IClient07DCUnion0
{
    /// <summary>
    /// The lowest level.
    /// </summary>
    [UInt(32)] public uint Min { get; private set; }

    /// <summary>
    /// The highest level; the client sends 0x7FFFFFFF when none is given.
    /// </summary>
    [UInt(32)] public uint Max { get; private set; }
}
