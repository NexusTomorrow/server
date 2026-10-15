using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerRecruitmentGuildDescription)]
[WireMessage(GameMessageOpcode.ServerRecruitmentGuildDescription, Direction.ServerToClient)]
public sealed partial class ServerRecruitmentGuildDescription : IWritable
{
    public Identity GuildIdentity { get; set; }
    [WideString] public string Description { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.WriteStringWide(Description);
    }
}
