using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.ProtocolReference.Tests.Message.Model.Sample;

/// <summary>
/// Asks for one page of the sample list.
/// </summary>
/// <remarks>
/// The server answers with <c>ServerFF21</c>.
/// </remarks>
[WireMessage((GameMessageOpcode)0xFF20, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class SampleRequest
{
    /// <summary>
    /// The page wanted, counted from 0.
    /// </summary>
    [UInt(8)] public byte Page { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It always sends 0.
    /// </summary>
    [UInt(4)] public byte Unknown1 { get; set; }
}

[WireStruct]
public sealed partial class ServerFF21Part0
{
    [UInt(32)] public uint Unknown0 { get; set; }

    [WideString] public string Unknown1 { get; set; } = "";
}

public interface IServerFF21Union0 : IWireStruct
{
}

[WireStruct]
[UnionCase(1)]
public sealed partial class ServerFF21Arm01 : IServerFF21Union0
{
}

[WireStruct]
[UnionCase(-1)]
public sealed partial class ServerFF21Arm07 : IServerFF21Union0
{
    [UInt(16)] public ushort Unknown0 { get; set; }
}

[WireMessage((GameMessageOpcode)0xFF21, Direction.ServerToClient)]
public sealed partial class ServerFF21
{
    [Array(CountBits = 4)] public ServerFF21Part0[] Unknown0 { get; set; } = [];

    [SInt(3)] public sbyte Unknown1 { get; set; }

    [Union(nameof(Unknown1))] public IServerFF21Union0 Unknown2 { get; set; } = new ServerFF21Arm01();
}

/// <summary>
/// A copy of <c>ServerFF21</c> with documentation and no rung, so its page shows the layout alone.
/// </summary>
/// <remarks>
/// Nothing reads these remarks below Purpose.
/// </remarks>
[WireMessage((GameMessageOpcode)0xFF22, Direction.ServerToClient)]
public sealed partial class ServerFF22
{
    /// <summary>
    /// A summary the page leaves out below Fields.
    /// </summary>
    [Array(CountBits = 4)] public ServerFF21Part0[] Unknown0 { get; set; } = [];

    [SInt(3)] public sbyte Unknown1 { get; set; }

    [Union(nameof(Unknown1))] public IServerFF21Union0 Unknown2 { get; set; } = new ServerFF21Arm01();
}

[WireMessage((GameMessageOpcode)0xFF23, Direction.ServerToClient)]
[LayoutConfirmed]
public sealed partial class ServerFF23
{
    [UInt(32)] public uint Unknown0 { get; set; }
}

[WireMessage((GameMessageOpcode)0xFF24, Direction.Both)]
[Dormant]
public sealed partial class MessageFF24
{
    [UInt(16)] public ushort Unknown0 { get; set; }
}
