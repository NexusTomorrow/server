using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Asks for the daily login rewards due; the client sends it only when a reward is due or the lockbox key's status is
/// <see cref="PremiumLockboxKeyStatus.Available"/>.
/// </summary>
[WireMessage(GameMessageOpcode.Client078F, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class DailyLoginRewardsRequest
{
}
