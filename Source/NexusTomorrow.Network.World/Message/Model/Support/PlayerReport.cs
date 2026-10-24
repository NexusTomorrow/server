using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Support;

/// <summary>
/// Reports a player: the ReportPlayer confirm action's request, sending the report a PrepareInfractionReport binding built.
/// </summary>
[WireMessage(GameMessageOpcode.Client06C5, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class PlayerReport
{
    /// <summary>
    /// The reported character, by realm and character id.
    /// </summary>
    public RealmScopedId PlayerId { get; private set; } = new();

    /// <summary>
    /// Why the player is reported.
    /// </summary>
    [UInt(3)] public ReportPlayerReason Reason { get; private set; }

    /// <summary>
    /// Where the player was reported from.
    /// </summary>
    [UInt(4)] public ReportPlayerSource Source { get; private set; }

    /// <summary>
    /// What was reported: a chat line, a mail's texts or an invite's text; empty for a unit, game, public event or housing report.
    /// </summary>
    [WideString] public string ReportedText { get; private set; } = "";

    /// <summary>
    /// The reported source's id, its kind following <see cref="Source"/>: a unit's id for a unit report, a mail's id for a mail report.
    /// </summary>
    [UInt(64)] public ulong SourceObjectId { get; private set; }

    [Float32] public float Unknown5 { get; private set; }

    /// <summary>
    /// True to ignore the player permanently as well.
    /// </summary>
    [UInt(1)] public bool PermanentIgnore { get; private set; }
}
