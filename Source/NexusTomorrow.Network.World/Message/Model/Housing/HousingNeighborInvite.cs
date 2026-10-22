using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Invites a player to be a neighbor: the request HousingLib.NeighborInviteByName sends, among others.
/// </summary>
/// <remarks>
/// By name the client sends the first two fields as 0; another sender sends a pair from the targeted unit and an empty name.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0512, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class HousingNeighborInvite
{
    [UInt(14)] public ushort Unknown0 { get; set; }

    [UInt(64)] public ulong Unknown1 { get; set; }
    /// <summary>
    /// The player, by name: HousingLib.NeighborInviteByName's argument.
    /// </summary>
    [WideString] public string PlayerName { get; set; } = "";
}
