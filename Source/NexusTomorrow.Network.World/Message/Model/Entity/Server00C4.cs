using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server00C4, NexusTomorrow.Network.Codec.Direction.ServerToClient)]
public sealed partial class Server00C4
{
    [UInt(8)] public byte Unknown0 { get; set; }
}
