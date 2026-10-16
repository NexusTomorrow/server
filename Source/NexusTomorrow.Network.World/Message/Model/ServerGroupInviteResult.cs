using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server041D, Direction.ServerToClient)]
public sealed partial class Server041D
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; }
    [UInt(5)] public byte Unknown2 { get; set; }
}
