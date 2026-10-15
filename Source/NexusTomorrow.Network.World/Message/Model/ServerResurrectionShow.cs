using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerResurrectionShow)]
[WireMessage(GameMessageOpcode.ServerResurrectionShow, Direction.ServerToClient)]
public sealed partial class ServerResurrectionShow : IWritable
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

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GhostId);
        writer.Write(RezCost);
        writer.Write(TimeUntilRezMs);
        writer.Write(Dead);
        writer.Write(ShowRezFlags, 8u);
        writer.Write(HasCasterRezRequest);
        writer.Write(TimeUntilWakeHereMs);
        writer.Write(TimeUntilForceRezMs);
    }
}
