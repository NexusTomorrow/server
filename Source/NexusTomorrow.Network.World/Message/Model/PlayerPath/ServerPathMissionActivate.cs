using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathMissionActivate)]
[WireMessage(GameMessageOpcode.ServerPathMissionActivate, Direction.ServerToClient)]
public sealed partial class ServerPathMissionActivate : IWritable
{
    [Array(CountBits = 32)] public List<Mission> Missions { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Missions.Count);
        Missions.ForEach(e => e.Write(writer));
    }
}
