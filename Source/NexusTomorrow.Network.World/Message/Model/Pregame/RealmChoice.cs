using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The player's realm choice on the realm-select screen.
/// </summary>
/// <remarks>
/// For the realm the client is already on, it carries on in character select without waiting; for another realm it waits for
/// <see cref="RealmRedirect"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client07DF, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class RealmChoice
{
    /// <summary>
    /// The chosen realm's id from <see cref="RealmList"/>, or the current realm's when the player backs out of realm select.
    /// </summary>
    [UInt(32)] public uint RealmId { get; private set; }
}
