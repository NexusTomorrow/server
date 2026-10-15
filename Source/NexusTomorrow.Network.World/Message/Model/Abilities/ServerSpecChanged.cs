using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ServerSpecChanged, Direction.ServerToClient)]
public sealed partial class ServerSpecChanged
{
    [UInt(3)] public byte ActionSetIndex { get; set; }
    [UInt(32)] public SpecError SpecError { get; set; }
}
