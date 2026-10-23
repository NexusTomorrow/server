using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Answers the player's request to change paths (<see cref="PathChangeRequest"/>).
/// </summary>
/// <remarks>
/// The client raises PathChangeResult with the result, which its add-ons compare with GameLib.CodeEnumGenericError.Ok. On a failure it
/// forgets the change it asked for.
/// </remarks>
[WireMessage(GameMessageOpcode.Server06B3, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PathChangeResult
{
    /// <summary>
    /// The result, a GameLib.CodeEnumGenericError code.
    /// </summary>
    [UInt(8)] public byte Result { get; set; }
}
