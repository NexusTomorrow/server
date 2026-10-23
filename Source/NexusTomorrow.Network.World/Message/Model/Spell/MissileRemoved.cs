using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Removes a missile.
/// </summary>
[WireMessage(GameMessageOpcode.Server077D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MissileRemoved
{
    /// <summary>
    /// The missile, by the id <see cref="MissileAdded"/> gave it.
    /// </summary>
    [UInt(32)] public uint MissileId { get; set; }
}
