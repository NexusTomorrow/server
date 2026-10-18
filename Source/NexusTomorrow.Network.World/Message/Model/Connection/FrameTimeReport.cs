using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Connection;

/// <summary>
/// Reports how long the client's frames take, in milliseconds.
/// </summary>
[WireMessage(GameMessageOpcode.Client0240, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class FrameTimeReport
{
    [UInt(1)] public bool Unknown0 { get; private set; }

    /// <summary>
    /// The average time of the client's recent frames, in milliseconds.
    /// </summary>
    [UInt(31)] public uint Unknown1 { get; private set; }

    [UInt(1)] public bool Unknown2 { get; private set; }

    /// <summary>
    /// The longest of those frames, in milliseconds.
    /// </summary>
    [UInt(31)] public uint Unknown3 { get; private set; }

    /// <summary>
    /// A point in world coordinates the client keeps with that longest frame.
    /// </summary>
    public Client0096Part0 Unknown4 { get; private set; } = new Client0096Part0();

    [Float32] public float Unknown5 { get; private set; }
    [UInt(1)] public bool Unknown6 { get; private set; }
    [UInt(31)] public uint Unknown7 { get; private set; }
}
