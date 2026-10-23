using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Tells the player that the group's leader has moved into another phase.
/// </summary>
/// <remarks>
/// The client raises GroupLeaderPhaseChanged with the three fields.
/// </remarks>
[WireMessage(GameMessageOpcode.Server042A, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GroupLeaderPhaseChanged
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
}
