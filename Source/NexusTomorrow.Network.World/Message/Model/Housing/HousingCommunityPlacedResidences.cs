using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Lists the residences placed in the player's community.
/// </summary>
/// <remarks>
/// The client raises HousingCommunityPlacedResidencesListRecieved with each residence's owner and plot.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04DD, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class HousingCommunityPlacedResidences
{
    /// <summary>
    /// One placed residence.
    /// </summary>
    [WireStruct]
    public sealed partial class PlacedResidence
    {
        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        public RealmScopedId Unknown0 { get; set; } = new();

        /// <summary>
        /// The residence's owner, by name: the event's strPlayerName.
        /// </summary>
        [WideString] public string PlayerName { get; set; } = "";

        /// <summary>
        /// The plot it occupies, from 0: the event's nPropertyIndex.
        /// </summary>
        [UInt(32)] public uint PropertyIndex { get; set; }
    }

    /// <summary>
    /// The placed residences.
    /// </summary>
    [Array(CountBits = 32)] public List<PlacedResidence> Residences { get; set; } = [];
}
