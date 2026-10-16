using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The account's subscription state. While it says the subscription has expired, the client refuses to create, delete, rename or
/// select characters.
/// </summary>
[WireMessage(GameMessageOpcode.Server033D, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SubscriptionState
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
}
