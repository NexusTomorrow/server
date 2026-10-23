using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Marks a tower defense improvement built in a soldier event.
/// </summary>
/// <remarks>
/// The client takes it only while the event is in <see cref="PlayerPathSoldierEventMode.Setup"/>. It adds the id to the event's built
/// improvements, which SoldierImprovement:IsBuilt reads, and raises UpdateSoldierBuild.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0681, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SoldierImprovementBuilt
{
    /// <summary>
    /// The soldier event, a PathSoldierEvent table id.
    /// </summary>
    [UInt(14)] public ushort SoldierEventId { get; set; }

    /// <summary>
    /// The improvement, a PathSoldierTowerDefense table id: one of the ids <see cref="SoldierBuildInvoke.BuiltImprovementIds"/> carries in
    /// 32 bits.
    /// </summary>
    [UInt(14)] public ushort ImprovementId { get; set; }
}
