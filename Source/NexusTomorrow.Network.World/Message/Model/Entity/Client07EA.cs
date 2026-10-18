using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Client07EA, Direction.ClientToServer)]
public sealed partial class Client07EA
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(7)] public byte Unknown1 { get; private set; }
}
