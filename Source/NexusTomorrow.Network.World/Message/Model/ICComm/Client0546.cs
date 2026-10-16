using NexusTomorrow.Game.Static.ICComm;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.Client0546, Direction.ClientToServer)]
public sealed partial class Client0546
{
    [UInt(3)] public byte Unknown0 { get; private set; }
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [WideString] public string Unknown2 { get; private set; }
}
