using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

/// <summary>
/// Reports an error to the player by its code.
/// </summary>
/// <remarks>
/// The client looks up its text for the code and raises GenericError with the code and the text.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0106, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GenericErrorNotify
{
    /// <summary>
    /// The error, a GameLib.CodeEnumGenericError code.
    /// </summary>
    [UInt(8)] public byte Error { get; set; }
}
