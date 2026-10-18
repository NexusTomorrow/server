using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 20.
/// </summary>
[WireStruct]
[UnionCase(20)]
public sealed partial class Server0262Union0Arm14 : IServer0262Union0
{
    /// <summary>
    /// With <see cref="Unknown1"/>, the key under which the client files the unit's name, and the unit's id when that id's top two bits are
    /// clear. The client refuses the unit when it is 0.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong Unknown0 { get; set; }

    /// <summary>
    /// With <see cref="Unknown0"/>, the key under which the client files the unit's name, and the unit's id when that id's top two bits are
    /// clear. The client refuses the unit when it is 0.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort Unknown1 { get; set; }

    /// <summary>
    /// The unit's name. When it is empty the client uses a default name.
    /// </summary>
    [VerifiedMember, WideString] public string Unknown2 { get; set; } = "";

    /// <summary>
    /// The unit's race id. The client refuses the unit when it is above 17.
    /// </summary>
    [VerifiedMember, UInt(5)] public byte Unknown3 { get; set; }

    /// <summary>
    /// The unit's class, as an id in the client's Class table. An id the table lacks does not refuse the unit.
    /// </summary>
    [VerifiedMember, UInt(5)] public byte Unknown4 { get; set; }

    /// <summary>
    /// The unit's gender id.
    /// </summary>
    [VerifiedMember, UInt(2)] public byte Unknown5 { get; set; }

    [UInt(64)] public ulong Unknown6 { get; set; }
    [Array(CountBits = 8), UInt(32)] public List<uint> Unknown7 { get; } = [];
    [WideString] public string Unknown8 { get; set; } = "";
    [UInt(4)] public byte Unknown9 { get; set; }
    [Array(CountBits = 5), UInt(64)] public List<ulong> Unknown10 { get; set; } = [];

    /// <summary>
    /// Values the client applies to the unit's model.
    /// </summary>
    [Array(CountBits = 6), Float32] public List<float> Unknown11 { get; set; } = [];

    [UInt(3)] public byte Unknown12 { get; set; }
    [UInt(8)] public byte Unknown13 { get; set; }
    [UInt(14)] public ushort Unknown14 { get; set; }
}
