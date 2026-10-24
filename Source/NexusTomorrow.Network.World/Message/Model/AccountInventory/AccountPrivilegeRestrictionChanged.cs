using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Restricts one of the account's privileges for a time; the client raises AccountPrivilegeRestrictionUpdate.
/// </summary>
[WireMessage(GameMessageOpcode.Server0971, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountPrivilegeRestrictionChanged
{
    /// <summary>
    /// The privilege restricted.
    /// </summary>
    [UInt(3)] public AccountPrivilegeRestriction Privilege { get; set; }

    /// <summary>
    /// How long the restriction lasts, in days, which the client counts from the message's arrival.
    /// </summary>
    [Float32] public float Days { get; set; }
}
