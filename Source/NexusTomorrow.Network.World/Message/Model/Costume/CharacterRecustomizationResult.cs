using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Reports the result of a character appearance change: the client raises CharacterRecustomizationResult with
/// <see cref="Result"/>.
/// </summary>
[WireMessage(GameMessageOpcode.Server0145, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CharacterRecustomizationResult
{
    /// <summary>
    /// The recustomization result, which the client raises as its CharacterRecustomizationResult event.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte Result { get; set; }
}
