using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Empties the player's datacube library.
/// </summary>
/// <remarks>
/// The client raises DatacubeUpdated.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00DF, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class DatacubesCleared
{
}
