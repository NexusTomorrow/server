using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.Message.Model.Shared;

namespace NexusTomorrow.Network.Auth.Message.Model;

/// <summary>
/// The realm's current notices, each in several languages.
/// </summary>
/// <remarks>
/// Arrives inside an envelope; the client takes it in any state.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0763, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class RealmNotices
{
    /// <summary>
    /// The notices. The client replaces the ones it holds with these.
    /// </summary>
    [Array(CountBits = 32)] public List<RealmNotice> Notices { get; set; } = [];
}
