using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

/// <summary>
/// Unlocks a mount; the client looks the mount spell up, shows a chat line and raises MountUnlocked.
/// </summary>
[WireMessage(GameMessageOpcode.Server0129, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnlockMount
{
    /// <summary>
    /// The mount spell, as a Spell4 id; the client looks it up in the spell manager.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint MountSpell4Id { get; set; }
}

