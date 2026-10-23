using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Lists residences the player can visit at random.
/// </summary>
/// <remarks>
/// HousingLib.RequestRandomVisit sends a listed owner's id back as <see cref="HousingVisit.PlayerId"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0527, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HousingRandomResidenceList
{
    /// <summary>
    /// One residence.
    /// </summary>
    [WireStruct]
    public sealed partial class ResidenceListing
    {
        /// <summary>
        /// The residence's owner, by character id.
        /// </summary>
        public RealmScopedId OwnerId { get; set; } = new();

        /// <summary>
        /// The owner, by name: HousingLib.GetRandomResidenceList's strCharacterName.
        /// </summary>
        [WideString] public string OwnerName { get; set; } = "";

        /// <summary>
        /// The residence's name: HousingLib.GetRandomResidenceList's strResidenceName.
        /// </summary>
        [WideString] public string ResidenceName { get; set; } = "";
    }

    /// <summary>
    /// The residences.
    /// </summary>
    [Array(CountBits = 32)] public List<ResidenceListing> Residences { get; set; } = [];
}
