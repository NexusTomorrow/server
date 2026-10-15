using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Achievement;

[Message(GameMessageOpcode.ServerAchievementInit)]
[WireMessage(GameMessageOpcode.ServerAchievementInit, Direction.ServerToClient)]
public sealed partial class ServerAchievementInit : IWritable
{
    [Array(CountBits = 32)] public List<Achievement> Achievements { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Achievements.Count);
        Achievements.ForEach(a => a.Write(writer));
    }
}
