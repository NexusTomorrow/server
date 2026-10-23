using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server06BB, Direction.ServerToClient)]
public sealed partial class PathMissionUpdate
{
    public PathMissionProgress Unknown0 { get; set; } = new();
}
