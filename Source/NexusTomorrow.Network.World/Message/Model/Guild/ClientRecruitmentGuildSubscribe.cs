using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ClientRecruitmentGuildSubscribe, Direction.ClientToServer)]
public sealed partial class ClientRecruitmentGuildSubscribe
{
    [UInt(1)] public bool Subscribe { get; private set; }
}
