using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Sets the community's privacy level: the request HousingLib.SetCommunityPrivacyLevel sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client0538, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class HousingSetCommunityPrivacyLevel
{
    /// <summary>
    /// The community, by id: the player's realm and the community guild's id.
    /// </summary>
    public RealmScopedId CommunityId { get; private set; } = new();

    [UInt(1)] public byte Unknown1 { get; private set; }
}
