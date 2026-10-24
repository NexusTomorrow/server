using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

/// <summary>
/// The 16042 client receives this message and ignores it.
/// </summary>
[WireMessage(GameMessageOpcode.Server03EF, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server03EF
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown0 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [Array(CountBits = 32), UInt(8)] public List<byte> Unknown1 { get; } = [];
}
