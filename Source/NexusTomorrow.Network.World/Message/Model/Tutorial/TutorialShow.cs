using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Tutorial;

/// <summary>
/// Shows a tutorial to the player. The client hands the id to its tutorial trigger, the same call it makes when a hazard is enabled.
/// </summary>
[WireMessage(GameMessageOpcode.Server0179, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class TutorialShow
{
    /// <summary>
    /// The tutorial, by id. The client looks it up in its tutorial registry and shows it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint TutorialId { get; set; }
}
