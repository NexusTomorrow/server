using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

/// <summary>
/// Asks for a realm's name.
/// </summary>
/// <remarks>
/// The client asks for a realm whose name it does not hold, for Unit.RequestRealmName and the friends list. The server answers with
/// <see cref="RealmNameResponse"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client059C, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class RealmNameRequest
{
    /// <summary>
    /// The realm, by realm id.
    /// </summary>
    [UInt(14)] public uint RealmId { get; private set; }
}
