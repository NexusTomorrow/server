using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

/// <summary>
/// Asks for a stuck player's recall: the GameLib.SupportStuck request, which the client sends only once its check finds the action's
/// recall can be cast.
/// </summary>
[WireMessage(GameMessageOpcode.Client0831, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class SupportStuck
{
    /// <summary>
    /// The recall the player chose.
    /// </summary>
    [VerifiedMember, UInt(3)] public SupportStuckAction Action { get; private set; }

    /// <summary>
    /// The recall's cast, by the client's own id for it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint ClientCastId { get; private set; }
}
