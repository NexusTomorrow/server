using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client010C, Direction.ClientToServer)]
public sealed partial class GuildSetHolomark
{
    [UInt(1)] public bool Unknown0 { get; private set; }
    [UInt(1)] public bool Unknown1 { get; private set; }
    [UInt(1)] public bool Unknown2 { get; private set; }
    [UInt(1)] public bool Unknown3 { get; private set; }
}
