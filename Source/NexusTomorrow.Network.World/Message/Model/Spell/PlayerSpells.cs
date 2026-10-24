using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Creates the player's spells.
/// </summary>
/// <remarks>
/// The client creates each spell on the player's unit and files it with the unit's spells, as it does for the spells a
/// <see cref="Entity.UnitCreate"/> carries.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0551, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PlayerSpells
{
    [Array(CountBits = 32)] public List<Server0262Part7> Unknown0 { get; } = [];
}
