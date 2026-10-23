using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Updates a settler infrastructure's status.
/// </summary>
/// <remarks>
/// The client finds the infrastructure's status, the one PlayerPathLib.GetInfrastructureStatusForMission reaches through a mission's
/// object id, changes its state to <see cref="State"/> when they differ, keeps <see cref="Percent"/>, and raises
/// SettlerInfrastructureUpdated.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0674, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SettlerInfrastructureUpdated
{
    /// <summary>
    /// The infrastructure, a PathSettlerInfrastructure table id.
    /// </summary>
    [UInt(32)] public uint InfrastructureId { get; set; }

    /// <summary>
    /// The infrastructure's state, which PlayerPathLib.GetCurrentSettlerInfrastructure and GetInfrastructureStatusForMission return as
    /// eState.
    /// </summary>
    [UInt(2)] public SettlerInfrastructureState State { get; set; }

    /// <summary>
    /// The infrastructure's progress, which PlayerPathLib.GetInfrastructureStatusForMission returns as nPercent.
    /// </summary>
    [Float32] public float Percent { get; set; }
}
