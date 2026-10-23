using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Opens a guild bank tab: the request GuildTypeLib's OpenBankTab sends, among others.
/// </summary>
/// <remarks>
/// The client sends it only for a tab it does not hold already.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0477, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GuildOpenBankTab
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; private set; } = new();

    /// <summary>
    /// The tab, as an inventory location: 100 to 109 a guild's tabs, 200 to 209 a war party's.
    /// </summary>
    [UInt(9)] public ushort Location { get; private set; }
}
