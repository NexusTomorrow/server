using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

/// <summary>
/// Reports a bug: the GameLib.ReportBug request, with the bug's subcategory and the unit and quest the player picked for it.
/// </summary>
[WireMessage(GameMessageOpcode.Client0830, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class BugReport
{
    /// <summary>
    /// The bug's subcategory, one of the ids GameLib.GetErrorCategories lists.
    /// </summary>
    [UInt(16)] public ushort SubcategoryId { get; private set; }

    /// <summary>
    /// The unit the bug concerns, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; private set; }

    /// <summary>
    /// The quest the bug concerns, as its Quest2 row id.
    /// </summary>
    [UInt(32)] public uint QuestId { get; private set; }

    [WideString] public string Unknown3 { get; private set; } = "";
}
