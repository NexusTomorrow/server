using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.Server0158, Direction.ServerToClient)]
public sealed partial class InstanceRestrictedDialogShow
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
