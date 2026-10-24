using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Tutorial;

/// <summary>
/// Offers a tutorial to the player. The client shows it only when the tutorial's Tutorial row allows it: its flags, level and prerequisite.
/// </summary>
[WireMessage(GameMessageOpcode.Server0179, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class TutorialShow
{
    /// <summary>
    /// The tutorial, by id in the Tutorial table.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint TutorialId { get; set; }
}
