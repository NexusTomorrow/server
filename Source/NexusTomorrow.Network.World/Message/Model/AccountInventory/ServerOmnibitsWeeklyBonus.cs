using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerOmnibitsWeeklyBonus, Direction.ServerToClient)]
public sealed partial class ServerOmnibitsWeeklyBonus
{
    [UInt(32)] public uint OmnibitsWeeklylBonusEarned { get; set; }
}
