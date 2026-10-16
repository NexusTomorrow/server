using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server048B, Direction.ServerToClient)]
public sealed partial class Server048B
{
    public Client00B5Part0 Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
