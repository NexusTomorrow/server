using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Sets a neighbor's permission: the request HousingLib.NeighborSetPermission sends, among others.
/// </summary>
[WireMessage(GameMessageOpcode.Client0518, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class HousingNeighborSetPermissionLevel
{
    /// <summary>
    /// The neighbor, by character id; zeros when the client names the neighbor instead.
    /// </summary>
    public RealmScopedId PlayerId { get; private set; } = new();

    [WideString] public string Unknown1 { get; private set; } = "";

    /// <summary>
    /// A HousingLib.NeighborPermissionLevel value: 0 normal, 1 roommate, 2 account.
    /// </summary>
    [UInt(32)] public uint Permission { get; private set; }
}
