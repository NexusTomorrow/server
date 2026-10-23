using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Reports a settler infrastructure started: the client raises SettlerInfrastructureStarted, with no argument.
/// </summary>
[WireMessage(GameMessageOpcode.Server0675, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SettlerInfrastructureStarted
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }
}
