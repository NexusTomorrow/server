using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server00B0, Direction.ServerToClient)]
public sealed partial class Server00B0
{
    [UInt(18)] public uint Unknown0 { get; set; }
}
