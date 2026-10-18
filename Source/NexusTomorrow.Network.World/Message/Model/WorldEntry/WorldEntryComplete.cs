using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Entity;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

/// <summary>
/// Tells the client it has finished entering the world.
/// </summary>
/// <remarks>
/// The client completes its world-entry checklist, which the loading screen waits on, and tells its interface the player has entered the
/// world. No other message completes it, so the client needs this one to leave the loading screen. It reads the player's unit without
/// checking it, so it comes after <see cref="PlayerUnit"/>; and it uses the zone id <see cref="WorldReady"/> carries, which the client
/// works out just before sending that, so it follows <see cref="WorldReady"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0061, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class WorldEntryComplete
{
}
