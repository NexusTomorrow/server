using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Asks to visit a residence or a community: the request FriendshipLib.VisitResidence, HousingLib.RequestRandomVisit,
/// HousingLib.RequestCommunityVisit and the other visit calls send.
/// </summary>
[WireMessage(GameMessageOpcode.Client0531, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class HousingVisit
{
    /// <summary>
    /// The client gives this value no meaning: every sender sends 0.
    /// </summary>
    [UInt(64)] public ulong Unknown0 { get; private set; }

    /// <summary>
    /// The residence to visit, by its owner's character id.
    /// </summary>
    public RealmScopedId PlayerId { get; } = new();

    [WideString] public string Unknown2 { get; private set; } = "";

    /// <summary>
    /// The community to visit, by id.
    /// </summary>
    public RealmScopedId CommunityId { get; } = new();

    [WideString] public string Unknown4 { get; private set; } = "";
}
