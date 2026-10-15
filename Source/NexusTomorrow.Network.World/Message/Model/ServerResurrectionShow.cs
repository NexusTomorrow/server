using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerResurrectionShow, Direction.ServerToClient)]
public sealed partial class ServerResurrectionShow
{
    [UInt(32)] public uint GhostId { get; set; }
    [UInt(32)] public uint RezCost { get; set; }

    /// <summary>
    /// Set the amount of time, in milliseconds,
    /// </summary>
    [UInt(32)] public uint TimeUntilRezMs { get; set; }

    [UInt(1)] public bool Dead { get; set; }
    [UInt(8)] public ResurrectionType ShowRezFlags { get; set; } // 8
    [UInt(1)] public bool HasCasterRezRequest { get; set; }

    /// <summary>
    /// This must be set if the player is to be given the option to use Service Tokens.
    /// </summary>
    [UInt(32)] public uint TimeUntilWakeHereMs { get; set; }

    [UInt(32)] public uint TimeUntilForceRezMs { get; set; }
}
