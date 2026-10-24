using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

/// <summary>
/// Reports that an instance reset failed: the client raises OnInstanceResetResult with false.
/// </summary>
[WireMessage(GameMessageOpcode.Server0154, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class InstanceResetFailed
{
    /// <summary>
    /// The client gives this value no meaning: its handler never reads it.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }
}
