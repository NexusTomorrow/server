using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client044A, Direction.ClientToServer)]
public sealed partial class Client044A
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    [WideString] public string Unknown1 { get; set; }
}
