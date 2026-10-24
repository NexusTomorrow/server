using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sends a list of records for the player's unit.
/// </summary>
/// <remarks>
/// The client hands the player's unit and the list to the routine it also runs for the records a
/// <see cref="Entity.UnitCreate"/> carries.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0551, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PlayerSpells
{
    [Array(CountBits = 32)] public List<Server0262Part7> Unknown0 { get; } = [];
}
