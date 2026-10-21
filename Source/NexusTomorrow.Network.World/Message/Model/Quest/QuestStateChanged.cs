using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// Sets a quest's state in the client's log.
/// </summary>
/// <remarks>
/// The client then raises QuestStateChanged, or ContractStateChanged when the quest's Quest2 flags carry 0x80000.
/// </remarks>
[WireMessage(GameMessageOpcode.Server035C, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class QuestStateChanged
{
    /// <summary>
    /// The quest, as its Quest2 row id.
    /// </summary>
    [UInt(15)] public ushort QuestId { get; set; }

    /// <summary>
    /// The quest's new state, which QuestStateChanged passes as eState.
    /// </summary>
    [UInt(32)] public uint State { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }
}
