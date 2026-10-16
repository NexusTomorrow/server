using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.Server075A, Direction.ServerToClient)]
public sealed partial class Server075A
{
    public Server0738Part0 Unknown0 { get; set; }
}
