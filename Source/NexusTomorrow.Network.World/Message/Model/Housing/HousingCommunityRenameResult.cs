using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Reports the result of renaming a community: the answer to <see cref="HousingRenameCommunity"/>.
/// </summary>
/// <remarks>
/// The client raises CommunityRenameResult with the result, for results 0, 1, 14, 47, 56 and 65 only.
/// </remarks>
[WireMessage(GameMessageOpcode.Server024D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class HousingCommunityRenameResult
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    public RealmScopedId Unknown0 { get; set; } = new();

    /// <summary>
    /// The result, which the client passes to CommunityRenameResult.
    /// </summary>
    [UInt(7)] public byte Result { get; set; }
}
