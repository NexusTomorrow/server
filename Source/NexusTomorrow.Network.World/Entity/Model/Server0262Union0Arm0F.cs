using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

/// <summary>
/// The data of a unit of type 15.
/// </summary>
[WireStruct]
[UnionCase(15)]
public sealed partial class Server0262Union0Arm0F : IServer0262Union0
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(15)] public ushort Unknown1 { get; set; }
    [UInt(6)] public byte Unknown2 { get; set; }
}
