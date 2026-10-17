using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A unit the client records this unit on, with two values.
/// </summary>
[WireStruct]
public sealed partial class Server0262Part0
{
    [UInt(2)] public byte Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }

    /// <summary>
    /// A unit's id. If the client has that unit, it records this unit on it, with the two values before this one.
    /// </summary>
    [UInt(32)] public uint Unknown2 { get; set; }
}
