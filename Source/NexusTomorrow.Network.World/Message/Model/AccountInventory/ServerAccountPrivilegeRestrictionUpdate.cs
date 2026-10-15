using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerAccountPrivilegeRestrictionUpdate, Direction.ServerToClient)]
public sealed partial class ServerAccountPrivilegeRestrictionUpdate
{
    [UInt(3)] public AccountPrivilegeRestriction Restriction { get; set; }
    [Float32] public float PenaltyDays { get; set; }
}
