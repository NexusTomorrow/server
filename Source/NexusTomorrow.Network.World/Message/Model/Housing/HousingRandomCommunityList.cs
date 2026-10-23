using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Lists communities the player can visit at random.
/// </summary>
/// <remarks>
/// HousingLib.RequestRandomCommunityVisit sends a listed community's id back as <see cref="HousingVisit.CommunityId"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0526, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HousingRandomCommunityList
{
    /// <summary>
    /// One community.
    /// </summary>
    [WireStruct]
    public sealed partial class CommunityListing
    {
        /// <summary>
        /// The community, by id.
        /// </summary>
        public RealmScopedId CommunityId { get; set; } = new();

        /// <summary>
        /// The community's name: HousingLib.GetRandomCommunityList's strResidenceName.
        /// </summary>
        [WideString] public string ResidenceName { get; set; } = "";

        /// <summary>
        /// Its leader, by name: HousingLib.GetRandomCommunityList's strCommunityLeader.
        /// </summary>
        [WideString] public string LeaderName { get; set; } = "";
    }

    /// <summary>
    /// The communities.
    /// </summary>
    [Array(CountBits = 32)] public List<CommunityListing> Communities { get; set; } = [];
}
