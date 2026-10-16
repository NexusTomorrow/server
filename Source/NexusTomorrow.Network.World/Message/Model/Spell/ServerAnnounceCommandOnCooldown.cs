using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0832, Direction.ServerToClient)]
public sealed partial class Server0832
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
