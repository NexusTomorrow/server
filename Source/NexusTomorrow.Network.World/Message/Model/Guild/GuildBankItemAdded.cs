using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Adds an item to a guild bank tab.
/// </summary>
/// <remarks>
/// The client puts the item in its tab's slot and raises GuildBankItem.
/// </remarks>
[WireMessage(GameMessageOpcode.Server046F, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildBankItemAdded
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The item, with the bank slot it sits in.
    /// </summary>
    public ItemInstance Item { get; set; } = new();
}
