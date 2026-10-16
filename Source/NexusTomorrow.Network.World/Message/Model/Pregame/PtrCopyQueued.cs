using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Tells the client a copy of a character to the test realm was queued.
/// </summary>
/// <remarks>
/// On character select the client passes it to its interface.
/// </remarks>
[WireMessage(GameMessageOpcode.Server06EA, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PtrCopyQueued
{
}
