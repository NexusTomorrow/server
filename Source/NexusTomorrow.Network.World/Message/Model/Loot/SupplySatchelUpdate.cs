using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

/// <summary>
/// Sets the amount of a supply in the player's satchel, which the client reports through its tradeskill interface.
/// </summary>
[WireMessage(GameMessageOpcode.Server0199, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SupplySatchelUpdate
{
    /// <summary>
    /// The supply's slot in the satchel.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort SlotIndex { get; set; }

    /// <summary>
    /// How many of the supply the satchel holds.
    /// </summary>
    [VerifiedMember, UInt(16)] public ushort Amount { get; set; }
}
