using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets a player unit's path.
/// </summary>
[WireMessage(GameMessageOpcode.Server08B8, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitPathTypeChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The path: Soldier 0, Settler 1, Scientist 2, Explorer 3.
    /// </summary>
    [UInt(3)] public byte PathType { get; set; }
}
