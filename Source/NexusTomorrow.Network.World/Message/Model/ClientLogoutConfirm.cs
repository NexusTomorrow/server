using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Called from ExitNow and ConfirmCamp lua functions
[Message(GameMessageOpcode.ClientLogoutConfirm)]
public class ClientLogoutConfirm : IReadable
{
    public void Read(GamePacketReader reader)
    {
    }
}
