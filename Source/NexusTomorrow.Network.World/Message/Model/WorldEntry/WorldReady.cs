using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.PlayerPath;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

/// <summary>
/// Tells the server the client has loaded the world and its player.
/// </summary>
/// <remarks>
/// Sent once per world by the client's world-entry checklist, once the world is loaded, <see cref="PlayerUnit"/> has named the player and
/// <see cref="WorldSettings"/> has arrived. The client then stays on the loading screen until <see cref="WorldEntryComplete"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00F2, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class WorldReady
{
    /// <summary>
    /// A zone id the client takes from the unit it moves: its zone lookups use it with the world and the player's position, and it reports
    /// changes of it in <see cref="ZoneChange"/>.
    /// </summary>
    [UInt(15)] public ushort ZoneId { get; private set; }
}
