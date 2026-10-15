using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildResult)]
[WireMessage(GameMessageOpcode.ServerGuildResult, Direction.ServerToClient)]
public partial class ServerGuildResult : IWritable
{
    public Identity GuildIdentity { get; set; }

    /// <summary>
    /// If the operation was a rank update, then this value is the index of the rank that was updated.
    /// Otherwise, this can be 1 or 0.
    /// </summary>
    [UInt(32)] public uint ReferenceId { get; set; }
    [WideString] public string TargetName { get; set; } // The name of the target of the operation.  This can be a character name or rank name.
    [UInt(8)] public GuildResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.Write(ReferenceId);
        writer.WriteStringWide(TargetName);
        writer.Write(Result, 8u);
    }
}
