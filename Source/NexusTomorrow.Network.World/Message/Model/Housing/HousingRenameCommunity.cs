using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Renames the community: the request the client sends when the player confirms the new name.
/// </summary>
[WireMessage(GameMessageOpcode.Client0788, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class HousingRenameCommunity
{
    /// <summary>
    /// The community, by id: the player's realm and the community guild's id.
    /// </summary>
    public RealmScopedId CommunityId { get; } = new();

    [WideString] public string Unknown1 { get; private set; } = "";
    [UInt(1)] public bool Unknown2 { get; private set; }
}
