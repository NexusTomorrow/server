using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The realms a character may transfer to, the answer to <see cref="TransferDestinationsRequest"/>.
/// </summary>
/// <remarks>
/// The client registers a writer for it as well, and no client code sends it.
/// </remarks>
[WireMessage(GameMessageOpcode.Message0862, Direction.Both)]
[Verified(Rung.Fields)]
public sealed partial class TransferDestinationList
{
    /// <summary>
    /// One realm a character may transfer to.
    /// </summary>
    [WireStruct]
    public sealed partial class TransferDestination : RealmListEntry
    {
        /// <summary>
        /// Whether the transfer to this realm costs nothing, as the client shows it.
        /// </summary>
        [UInt(1)] public bool IsFree { get; set; }
    }

    /// <summary>
    /// The realms.
    /// </summary>
    [Array(CountBits = 32)] public List<TransferDestination> Destinations { get; set; } = [];
}
