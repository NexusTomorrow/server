using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets a unit's name and title.
/// </summary>
[WireMessage(GameMessageOpcode.Server0939, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitNameTitleChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The title, a CharacterTitle table id.
    /// </summary>
    [UInt(14)] public ushort TitleId { get; set; }

    /// <summary>
    /// A creature whose name the unit takes in place of <see cref="Name"/>, a Creature2 table id; 0 for none.
    /// </summary>
    [UInt(18)] public uint CreatureId { get; set; }

    /// <summary>
    /// The unit's name, unless a creature gives one.
    /// </summary>
    [WideString] public string Name { get; set; } = "";
}
