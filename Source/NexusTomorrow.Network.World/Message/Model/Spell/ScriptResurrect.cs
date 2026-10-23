using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Tells the client of a scripted resurrection.
/// </summary>
/// <remarks>
/// The client raises ScriptResurrect.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0160, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ScriptResurrect
{
}
