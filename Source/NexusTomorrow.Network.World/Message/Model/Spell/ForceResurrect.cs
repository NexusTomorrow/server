using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Tells the client of a forced resurrection.
/// </summary>
/// <remarks>
/// The client raises ForceResurrect.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0101, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ForceResurrect
{
}
