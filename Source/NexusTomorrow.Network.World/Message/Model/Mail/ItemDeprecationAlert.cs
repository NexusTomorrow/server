using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[WireMessage(GameMessageOpcode.Server0566, Direction.ServerToClient)]
public sealed partial class ItemDeprecationAlert
{
    [Array(CountBits = 32), UInt(64)] public List<ulong> Unknown0 { get; set; } = [];
}
