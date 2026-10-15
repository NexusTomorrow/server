using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingCommunityRename, Direction.ServerToClient)]
public sealed partial class ServerHousingCommunityRename
{
    public Identity TargetGuild { get; set; } // Not used in the client
    [UInt(7)] public HousingResult Result { get; set; }
}
