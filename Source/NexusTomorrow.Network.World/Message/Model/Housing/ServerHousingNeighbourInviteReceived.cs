using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingNeighbourInviteReceived)]
[WireMessage(GameMessageOpcode.ServerHousingNeighbourInviteReceived, Direction.ServerToClient)]
public sealed partial class ServerHousingNeighbourInviteReceived : IWritable
{
    public Identity Invitor { get; set; } // Not sure what is being identified here, not used by client
    [WideString] public string InvitorName { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Invitor.Write(writer);
        writer.WriteStringWide(InvitorName);
    }
}
