using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 27: the unit's movement mode.
/// </summary>
[WireStruct]
[UnionCase(27)]
public sealed partial class SetModeCommand : IServer0262Union2
{
    /// <summary>
    /// The unit's movement mode. It holds until the next mode command. The client sends 0 to 3 for the unit it moves.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Mode { get; set; }
}
