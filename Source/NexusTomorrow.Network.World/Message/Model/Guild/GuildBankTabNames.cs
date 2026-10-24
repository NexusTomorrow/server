using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Names a guild's bank tabs.
/// </summary>
/// <remarks>
/// GuildTypeLib's GetBankTabName returns each; the client raises GuildBankTabRename.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0479, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildBankTabNames
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// Each bank tab's name, in tab order.
    /// </summary>
    [FixedArray(10), WideString] public string[] TabNames { get; set; } = [.. Enumerable.Range(0, 10).Select(_ => "")];
}
