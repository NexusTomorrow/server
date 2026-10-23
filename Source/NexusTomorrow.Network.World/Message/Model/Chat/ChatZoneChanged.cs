using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// Tells the client which zone the player is in, for its zone channels' names.
/// </summary>
/// <remarks>
/// The client renames every zone channel after the zone and raises ChatZoneChange.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01DA, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ChatZoneChanged
{
    /// <summary>
    /// The zone, a WorldZone table id.
    /// </summary>
    [UInt(15)] public ushort ZoneId { get; set; }
}
