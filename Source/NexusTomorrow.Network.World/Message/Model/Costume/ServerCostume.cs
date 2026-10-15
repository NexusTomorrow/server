using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ServerCostume)]
[WireMessage(GameMessageOpcode.ServerCostume, Direction.ServerToClient)]
public sealed partial class ServerCostume : IWritable
{
    public Costume Costume { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Costume.Write(writer);
    }
}
