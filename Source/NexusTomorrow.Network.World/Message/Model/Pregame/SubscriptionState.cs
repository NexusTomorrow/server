using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The account's subscription state.
/// </summary>
[WireMessage(GameMessageOpcode.Server033D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SubscriptionState
{
    /// <summary>
    /// Bit 0 set makes the interface offer a free realm transfer; the client reads no other bit.
    /// </summary>
    [VerifiedMember]
    [UInt(32)] public uint TransferFlags { get; set; }

    /// <summary>
    /// Whether the subscription has expired. While it has, the client refuses to create, delete, rename or select characters and to
    /// ask for transfer destinations.
    /// </summary>
    [VerifiedMember]
    [UInt(1)] public bool Expired { get; set; }

    /// <summary>
    /// Hours of game time remaining, as a float. The client multiplies it by 3.6e10 (100-ns units per hour) and adds the current
    /// time to get the expiry, then raises GameTimeHoursRemaining.
    /// </summary>
    [VerifiedMember]
    [Float32] public float HoursRemaining { get; set; }
}
