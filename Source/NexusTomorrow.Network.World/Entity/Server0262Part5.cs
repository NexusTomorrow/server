using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Entity.Command;

namespace NexusTomorrow.Network.World.Entity;

/// <summary>
/// One command applied to a unit: which command, and its value.
/// </summary>
[WireStruct]
public sealed partial class Server0262Part5
{
    /// <summary>
    /// Which command, 0 to 29. It selects <see cref="Unknown1"/>.
    /// </summary>
    [VerifiedMember, UInt(5)] public byte Unknown0 { get; set; }

    /// <summary>
    /// The command's value, in the form <see cref="Unknown0"/> selects.
    /// </summary>
    [VerifiedMember, Union(nameof(Unknown0))] public IServer0262Union2 Unknown1 { get; set; } = new Server0262Union2Arm00();
}
