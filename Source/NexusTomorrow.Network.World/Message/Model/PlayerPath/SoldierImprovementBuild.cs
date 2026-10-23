using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Builds a tower defense improvement in a soldier event.
/// </summary>
/// <remarks>
/// SoldierImprovement.Build sends it, only for an event of PathSoldierEvent type 4 in <see cref="PlayerPathSoldierEventMode.Setup"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client067D, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class SoldierImprovementBuild
{
    /// <summary>
    /// The improvement, a PathSoldierTowerDefense table id: the id SoldierImprovement.IsBuilt looks for.
    /// </summary>
    [UInt(14)] public ushort ImprovementId { get; private set; }
}
