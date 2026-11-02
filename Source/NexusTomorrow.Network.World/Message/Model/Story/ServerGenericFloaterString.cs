using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

/// <summary>
/// A generic floater string; the client raises a GenericFloater event for the player with this text.
/// </summary>
[WireMessage(GameMessageOpcode.Server0724, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ServerGenericFloaterString
{
    /// <summary>
    /// The floater text.
    /// </summary>
    [VerifiedMember, WideString] public string Text { get; set; } = "";
}