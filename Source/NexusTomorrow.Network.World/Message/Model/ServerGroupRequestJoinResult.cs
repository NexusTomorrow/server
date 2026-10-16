using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0456, Direction.ServerToClient)]
public sealed partial class Server0456
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    [WideString] public string Unknown1 { get; set; }

    [UInt(5)] public byte Unknown2 { get; set; }

    [UInt(1)] public bool Unknown3 { get; set; }
}
