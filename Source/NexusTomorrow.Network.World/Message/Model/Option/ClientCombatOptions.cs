using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[WireMessage(GameMessageOpcode.Client00D5, Direction.ClientToServer)]
public sealed partial class Client00D5
{
    [UInt(4)] public byte Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    [UInt(14)] public ushort Unknown2 { get; set; }
}
