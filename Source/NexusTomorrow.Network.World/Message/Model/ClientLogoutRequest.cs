using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Called from RequestCamp, ExitGame, Camp, CancelExit lua functions
[Message(GameMessageOpcode.ClientLogoutRequest)]
[WireMessage(GameMessageOpcode.ClientLogoutRequest, Direction.ClientToServer)]
public sealed partial class ClientLogoutRequest : IReadable
{
    [UInt(1)] public bool Initiated { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Initiated = reader.ReadBit();
    }
}
