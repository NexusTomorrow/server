using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Client07DF, Direction.ClientToServer)]
public sealed partial class Client07DF
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
