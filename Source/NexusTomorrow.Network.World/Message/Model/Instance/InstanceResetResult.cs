using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

/// <summary>
/// Reports an instance reset's result: the client raises OnInstanceResetResult with <see cref="Success"/>.
/// </summary>
[WireMessage(GameMessageOpcode.Server0155, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class InstanceResetResult
{
    /// <summary>
    /// The client gives this value no meaning. Its handler reads only <see cref="Success"/>.
    /// </summary>
    [UInt(15)] public uint Unknown0 { get; set; }

    /// <summary>
    /// Whether the instance was reset.
    /// </summary>
    [UInt(1)] public bool Success { get; set; }
}
