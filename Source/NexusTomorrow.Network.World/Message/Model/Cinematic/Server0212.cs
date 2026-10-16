using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Server0212, Direction.ServerToClient)]
public sealed partial class Server0212
{
    [UInt(32)] public uint Unknown0 { get; set; }
    public Client0096Part0 Unknown1 { get; set; } = new();
}
