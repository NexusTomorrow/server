using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets a group's instance difficulty.
/// </summary>
/// <remarks>
/// The client stores it on whichever of its current and previous groups has the id.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0414, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupInstanceDifficultyChanged
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The group's instance difficulty, a GroupLib.Difficulty value (0 normal, 1 veteran), which GroupLib.GetInstanceDifficulty
    /// returns.
    /// </summary>
    [UInt(2)] public byte Difficulty { get; set; }
}
