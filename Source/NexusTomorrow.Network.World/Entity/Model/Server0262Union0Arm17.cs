using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 23.
/// </summary>
[WireStruct]
[UnionCase(23)]
public sealed partial class Server0262Union0Arm17 : IServer0262Union0
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }

    /// <summary>
    /// The unit's name. When it is empty the client uses a default name.
    /// </summary>
    [WideString] public string Unknown2 { get; set; } = "";

    /// <summary>
    /// The unit's race id. The client refuses the unit when it is above 17.
    /// </summary>
    [UInt(5)] public byte Unknown3 { get; set; }

    /// <summary>
    /// The unit's class, as an id in the client's Class table. An id the table lacks does not refuse the unit.
    /// </summary>
    [UInt(5)] public byte Unknown4 { get; set; }

    /// <summary>
    /// The unit's gender id.
    /// </summary>
    [UInt(2)] public byte Unknown5 { get; set; }

    [UInt(64)] public ulong Unknown6 { get; set; }
    [WideString] public string Unknown7 { get; set; } = "";
    [UInt(4)] public byte Unknown8 { get; set; }
    [Array(CountBits = 5), UInt(64)] public List<ulong> Unknown9 { get; set; } = [];
    [Array(CountBits = 6), Float32] public List<float> Unknown10 { get; set; } = [];
    [UInt(14)] public ushort Unknown11 { get; set; }
}
