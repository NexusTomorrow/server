using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Achievement;

[WireMessage(GameMessageOpcode.ServerAchievementInit, Direction.ServerToClient)]
public sealed partial class ServerAchievementInit
{
    [Array(CountBits = 32)] public List<Achievement> Achievements { get; set; } = [];
}
