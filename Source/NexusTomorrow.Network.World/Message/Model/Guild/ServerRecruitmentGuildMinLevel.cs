using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerRecruitmentGuildMinLevel)]
[WireMessage(GameMessageOpcode.ServerRecruitmentGuildMinLevel, Direction.ServerToClient)]
public sealed partial class ServerRecruitmentGuildMinLevel : IWritable
{
    public Identity GuildIdentity { get; set; }
    [UInt(32)] public uint MinimumLevel { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.Write(MinimumLevel);
    }
}
