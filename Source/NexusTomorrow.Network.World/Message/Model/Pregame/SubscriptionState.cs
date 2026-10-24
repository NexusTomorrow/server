using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The account's subscription state.
/// </summary>
[WireMessage(GameMessageOpcode.Server033D, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SubscriptionState
{
    /// <summary>
    /// Bit 0 set makes the interface offer a free realm transfer; the client reads no other bit.
    /// </summary>
    [UInt(32)] public uint TransferFlags { get; set; }

    /// <summary>
    /// Whether the subscription has expired. While it has, the client refuses to create, delete, rename or select characters and to
    /// ask for transfer destinations.
    /// </summary>
    [UInt(1)] public bool Expired { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }
}
