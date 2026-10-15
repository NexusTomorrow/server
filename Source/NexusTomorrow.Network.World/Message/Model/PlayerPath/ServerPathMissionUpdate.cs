using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathMissionUpdate, Direction.ServerToClient)]
public sealed partial class ServerPathMissionUpdate
{
    public MissionProgress Mission { get; set; } = new();
}
