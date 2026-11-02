using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// The player's wallet balance; the client updates it when it changes.
/// </summary>
[WireMessage(GameMessageOpcode.Server0980, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ServerWalletUpdate
{
    /// <summary>
    /// The wallet balance.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Balance { get; set; }
}
