using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Client0786, Direction.ClientToServer)]
public sealed partial class Client0786
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [WideString] public string Unknown1 { get; private set; }
}
