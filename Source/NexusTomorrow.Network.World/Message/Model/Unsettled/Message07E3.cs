using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

/// <summary>
/// The 16042 client receives this message and ignores it, and never sends it.
/// </summary>
[WireMessage(GameMessageOpcode.Message07E3, Direction.Both)]
[Dormant]
public sealed partial class Message07E3
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }
}
