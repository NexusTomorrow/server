using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Launches missiles.
/// </summary>
[WireMessage(GameMessageOpcode.Server0260, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MissilesAdded
{
    /// <summary>
    /// The missiles, each as <see cref="MissileAdded"/> launches one.
    /// </summary>
    [Array(CountBits = 32)] public List<MissileAdded> Missiles { get; set; } = [];
}

