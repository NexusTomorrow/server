using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingMatchInitiateLookingForReplacements)]
public class ClientMatchingMatchInitiateLookingForReplacements : IReadable
{
    public Role Roles { get; private set; } // Roles being looked for

    public void Read(GamePacketReader reader)
    {
        Roles = reader.ReadEnum<Role>(32u);
    }
}
