using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathMissionActivate, Direction.ServerToClient)]
public sealed partial class ServerPathMissionActivate
{
    [Array(CountBits = 32)] public List<Mission> Missions { get; set; } = [];
}
