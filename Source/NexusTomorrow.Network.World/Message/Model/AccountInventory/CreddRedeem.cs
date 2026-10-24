using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Asks to redeem a CREDD; the client sends it only while the account holds one.
/// </summary>
[WireMessage(GameMessageOpcode.Client0770, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CreddRedeem
{
}
