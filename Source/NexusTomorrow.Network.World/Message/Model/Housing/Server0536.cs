using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0536, Direction.ServerToClient)]
public sealed partial class Server0536
{
    public Client00B5Part0 Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; }
    [UInt(7)] public byte Unknown2 { get; set; }
}
