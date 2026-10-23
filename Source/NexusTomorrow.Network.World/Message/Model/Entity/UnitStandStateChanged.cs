using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets a unit's stand state.
/// </summary>
[WireMessage(GameMessageOpcode.Server093C, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitStandStateChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The stand state.
    /// </summary>
    [UInt(4)] public StandState StandState { get; set; }

    /// <summary>
    /// The emote the stand state comes from, an Emotes table id.
    /// </summary>
    [UInt(14)] public uint EmoteId { get; set; }
}
