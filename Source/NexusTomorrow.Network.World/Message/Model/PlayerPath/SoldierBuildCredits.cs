using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Sets a soldier event's build credits.
/// </summary>
/// <remarks>
/// The client takes it only while the event is in <see cref="PlayerPathSoldierEventMode.Setup"/>, and raises UpdateSoldierBuild.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0680, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SoldierBuildCredits
{
    /// <summary>
    /// The soldier event, a PathSoldierEvent table id.
    /// </summary>
    [UInt(14)] public ushort SoldierEventId { get; set; }

    /// <summary>
    /// The build credits, which SoldierEvent.GetBuildCredits returns.
    /// </summary>
    [UInt(32)] public uint Credits { get; set; }
}
