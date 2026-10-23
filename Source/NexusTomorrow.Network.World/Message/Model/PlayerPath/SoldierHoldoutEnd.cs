using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Ends a soldier event with its result.
/// </summary>
/// <remarks>
/// The client drops the event from the player's active events, clears the sound zone kit its wave set, and raises SoldierHoldoutEnd with the
/// event and the result.
/// </remarks>
[WireMessage(GameMessageOpcode.Server067C, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SoldierHoldoutEnd
{
    /// <summary>
    /// The soldier event, a PathSoldierEvent table id.
    /// </summary>
    [UInt(14)] public ushort SoldierEventId { get; set; }

    /// <summary>
    /// How the event ended.
    /// </summary>
    [UInt(32)] public PlayerPathSoldierResult Result { get; set; }
}
