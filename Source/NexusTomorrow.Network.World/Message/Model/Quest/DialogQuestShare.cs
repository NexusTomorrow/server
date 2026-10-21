using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// Offers the player a quest another unit shares.
/// </summary>
/// <remarks>
/// For a quest it knows, the client raises Dialog_QuestShare with the quest and the sharing unit.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0363, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class DialogQuestShare
{
    /// <summary>
    /// The unit sharing the quest, by unit id.
    /// </summary>
    [UInt(32)] public uint SharingUnitId { get; set; }

    /// <summary>
    /// The quest, as its Quest2 row id.
    /// </summary>
    [UInt(15)] public uint QuestId { get; set; }
}
