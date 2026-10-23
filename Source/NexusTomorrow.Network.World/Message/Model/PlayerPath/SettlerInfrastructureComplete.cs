using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Reports a settler infrastructure complete: the client raises SettlerInfrastructureComplete, with no argument.
/// </summary>
[WireMessage(GameMessageOpcode.Server0673, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SettlerInfrastructureComplete
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }
}
