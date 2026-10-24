using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Reports the result of a character appearance change: the client raises CharacterRecustomizationResult with
/// <see cref="Unknown0"/>.
/// </summary>
[WireMessage(GameMessageOpcode.Server0145, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CharacterRecustomizationResult
{
    [UInt(3)] public byte Unknown0 { get; set; }
}
