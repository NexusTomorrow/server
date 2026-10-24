using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

/// <summary>
/// Answers <see cref="RealmNameRequest"/> with a realm's name.
/// </summary>
/// <remarks>
/// For Unit.RequestRealmName the client raises PlayerRealmName with the name.
/// </remarks>
[WireMessage(GameMessageOpcode.Server059D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class RealmNameResponse
{
    /// <summary>
    /// The realm, as <see cref="RealmNameRequest.RealmId"/> gave it.
    /// </summary>
    [UInt(14)] public ushort RealmId { get; set; }

    /// <summary>
    /// The realm's name, which the client keeps and hands to the waiting callers.
    /// </summary>
    [WideString] public string Name { get; set; } = "";
}
