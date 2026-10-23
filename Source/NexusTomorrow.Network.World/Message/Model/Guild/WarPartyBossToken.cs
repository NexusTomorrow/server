using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// One kind of boss token a war party holds; GuildTypeLib's GetBossTokens skips a token with either value 0.
/// </summary>
[WireStruct]
public sealed partial class WarPartyBossToken
{
    /// <summary>
    /// The token, by item id.
    /// </summary>
    [UInt(18)] public uint ItemId { get; set; }

    /// <summary>
    /// How many the war party holds.
    /// </summary>
    [UInt(32)] public uint Count { get; set; }
}
