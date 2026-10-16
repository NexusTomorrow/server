using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0804, Direction.ServerToClient)]
public sealed partial class Server0804
{
    public Server0804Part0 Unknown0 { get; set; }
}
