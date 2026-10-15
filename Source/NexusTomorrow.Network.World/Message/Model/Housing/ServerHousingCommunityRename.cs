using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingCommunityRename)]
[WireMessage(GameMessageOpcode.ServerHousingCommunityRename, Direction.ServerToClient)]
public sealed partial class ServerHousingCommunityRename : IWritable
{
    public Identity TargetGuild { get; set; } // Not used in the client
    [UInt(7)] public HousingResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        TargetGuild.Write(writer);
        writer.Write(Result, 7u);
    }
}
