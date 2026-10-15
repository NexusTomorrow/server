using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingRoleCheckResponse)]
[WireMessage(GameMessageOpcode.ClientMatchingRoleCheckResponse, Direction.ClientToServer)]
public sealed partial class ClientMatchingRoleCheckResponse : IReadable
{
    [UInt(5)] public Game.Static.Matching.MatchType Type { get; private set; } // Uses ReadyMatchType sent in 0x05E3
    [UInt(32)] public Role Roles { get; private set; }
    [Pad(32)] public Pad RolesHigh { get; }
    [UInt(1)] public bool Response { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Type = reader.ReadEnum<Game.Static.Matching.MatchType>(5u);
        Roles = reader.ReadEnum<Role>();
        Response = reader.ReadBit();
    }
}
