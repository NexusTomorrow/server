using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The realm-select screen's list of realms, and the realm notices.
/// </summary>
/// <remarks>
/// The client registers a writer for it as well. When a client-side switch is set, the client selects a realm from the list on its own:
/// the first whose <see cref="RealmListEntry.Flags"/> has bit 0 set, else the first.
/// </remarks>
[WireMessage(GameMessageOpcode.Message0761, Direction.Both)]
[Verified(Rung.Fields)]
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
