using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The realm-select screen's list of realms, and the realm notices.
/// </summary>
/// <remarks>
/// The client registers a writer for it as well.
/// </remarks>
[WireMessage(GameMessageOpcode.Message0761, Direction.Both)]
[Verified(Rung.Purpose)]
public sealed partial class RealmList
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(64)] public ulong Unknown0 { get; set; }

    /// <summary>
    /// The realms. The client replaces its list with these.
    /// </summary>
    [Array(CountBits = 32)] public List<RealmListEntry> Realms { get; set; } = [];

    /// <summary>
    /// The realm notices. The client replaces the ones it holds with these.
    /// </summary>
    [Array(CountBits = 32)] public List<RealmNotice> Notices { get; set; } = [];
}
