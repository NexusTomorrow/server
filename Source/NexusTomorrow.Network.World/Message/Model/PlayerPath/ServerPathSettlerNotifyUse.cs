using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSettlerNotifyUse, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerNotifyUse
{
    [UInt(14)] public ushort PathSettlerImprovementGroupId { get; set; }
    [WideString] public string Name { get; set; }
}
