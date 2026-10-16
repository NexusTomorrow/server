using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0519, Direction.ServerToClient)]
public sealed partial class Server0519
{
    public Server0507Part0 Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
}
