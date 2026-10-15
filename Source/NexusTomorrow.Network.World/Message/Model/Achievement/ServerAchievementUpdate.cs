using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Achievement;

[WireMessage(GameMessageOpcode.ServerAchievementUpdate, Direction.ServerToClient)]
public sealed partial class ServerAchievementUpdate
{
    [UInt(1)] public bool Deleted { get; set; }
    [Array(CountBits = 32)] public List<Achievement> Achievements { get; set; } = [];
}
