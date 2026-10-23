using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Invokes the soldier build for an event at a unit, with the improvements already built.
/// </summary>
/// <remarks>
/// The client takes it only while the player is interacting as <see cref="InteractionType.SoldierBuild"/> and the event is in
/// <see cref="PlayerPathSoldierEventMode.Setup"/>. It adds the ids to the event's built improvements, which SoldierImprovement:IsBuilt
/// reads, and raises InvokeSoldierBuild with the unit and the event.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0676, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SoldierBuildInvoke
{
    /// <summary>
    /// The unit the build is invoked at, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The soldier event, a PathSoldierEvent table id.
    /// </summary>
    [UInt(14)] public ushort SoldierEventId { get; set; }

    /// <summary>
    /// The improvements built, PathSoldierTowerDefense table ids: the ids <see cref="SoldierImprovementBuilt.ImprovementId"/> carries in
    /// 14 bits, here in 32.
    /// </summary>
    [Array(CountBits = 32), UInt(32)] public List<uint> BuiltImprovementIds { get; set; } = [];
}
