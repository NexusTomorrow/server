using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Updates a settler infrastructure mission's status.
/// </summary>
/// <remarks>
/// The client finds the status PlayerPathLib.GetInfrastructureStatusForMission returns for the mission, changes its state to
/// <see cref="Unknown1"/> when they differ, keeps <see cref="Percent"/>, and raises SettlerInfrastructureUpdated.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0674, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SettlerInfrastructureUpdated
{
    /// <summary>
    /// The mission, by id, as PlayerPathLib.GetInfrastructureStatusForMission takes it.
    /// </summary>
    [UInt(32)] public uint MissionId { get; set; }

    [UInt(2)] public byte Unknown1 { get; set; }

    /// <summary>
    /// The infrastructure's progress, which PlayerPathLib.GetInfrastructureStatusForMission returns as nPercent.
    /// </summary>
    [Float32] public float Percent { get; set; }
}
