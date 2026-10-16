using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0818, Direction.ServerToClient)]
public sealed partial class Server0818
{
    [UInt(32)] public uint Unknown0 { get; set; }
    public Server0262Part8 Unknown1 { get; set; } = new();
}
