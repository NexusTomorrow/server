using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Achievement;

[WireMessage(GameMessageOpcode.ServerRealmFirstAchievement, Direction.ServerToClient)]
public sealed partial class ServerRealmFirstAchievement
{
    [UInt(15)] public ushort AchievementId { get; set; }
    [UInt(1)] public bool IsGuildAchievement { get; set; }
    [WideString] public string Name { get; set; } // player or guild name
}
