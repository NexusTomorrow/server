using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingMatchInitiateLookingForReplacements)]
[WireMessage(GameMessageOpcode.ClientMatchingMatchInitiateLookingForReplacements, Direction.ClientToServer)]
public sealed partial class ClientMatchingMatchInitiateLookingForReplacements : IReadable
{
    [UInt(32)] public Role Roles { get; private set; } // Roles being looked for

    public void Read(GamePacketReader reader)
    {
        Roles = reader.ReadEnum<Role>(32u);
    }
}
