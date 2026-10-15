using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ClientGuildHolomarkUpdate, Direction.ClientToServer)]
public sealed partial class ClientGuildHolomarkUpdate
{
    [UInt(1)] public bool LeftHidden { get; private set; }
    [UInt(1)] public bool RightHidden { get; private set; }
    [UInt(1)] public bool BackHidden { get; private set; }
    [UInt(1)] public bool DistanceNear { get; private set; }
}
