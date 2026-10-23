using System.Diagnostics.CodeAnalysis;
using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Joins a matching queue; MatchMakingLib.Queue sends it.
/// </summary>
[WireMessage(GameMessageOpcode.Client05EF, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "The request joins the matching queue; the type is not a collection.")]
public sealed partial class MatchingQueue
{
    /// <summary>
    /// The queue to join.
    /// </summary>
    public MatchingQueueRequest Request { get; private set; } = new();

    /// <summary>
    /// The roles queued for, as a mask of <see cref="Role"/>.
    /// </summary>
    [UInt(32)] public Role Roles { get; private set; }

    [UInt(32)] public uint Unknown2 { get; private set; }
}
