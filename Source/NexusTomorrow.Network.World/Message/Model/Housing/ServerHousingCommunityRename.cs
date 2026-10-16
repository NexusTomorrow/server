using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server024D, Direction.ServerToClient)]
public sealed partial class Server024D
{
    public Client00B5Part0 Unknown0 { get; set; }
    [UInt(7)] public byte Unknown1 { get; set; }
}
