using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A unit that takes this unit as its vehicle, with two values.
/// </summary>
[WireStruct]
public sealed partial class Server0262Part0
{
    [UInt(2)] public byte Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }

    /// <summary>
    /// A unit, by unit id. If the client has that unit, that unit takes this one as its vehicle, with the two values before this one.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }
}
