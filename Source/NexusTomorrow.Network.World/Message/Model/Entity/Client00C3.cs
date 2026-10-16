using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Client00C3, Direction.ClientToServer)]
public sealed partial class Client00C3
{
    [UInt(8)] public byte Unknown0 { get; private set; }
    [UInt(8)] public byte Unknown1 { get; private set; }
}
