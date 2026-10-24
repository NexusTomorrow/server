using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Starts playback of the announced cinematic: its commands run by their times from now, and its duration starts.
/// </summary>
/// <remarks>
/// While a screen fade or a sound the cinematic loads is still pending, the client defers the start.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0210, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicPlay
{
}
