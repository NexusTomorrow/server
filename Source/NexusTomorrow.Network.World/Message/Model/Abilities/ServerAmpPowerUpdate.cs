using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

/// <summary>
/// The player's Eldan Augmentation power; the client updates the augmentations from it.
/// </summary>
[WireMessage(GameMessageOpcode.Server01A1, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ServerAmpPowerUpdate
{
    /// <summary>
    /// The AMP power value.
    /// </summary>
    [VerifiedMember, SInt(16)] public short Power { get; set; }
}