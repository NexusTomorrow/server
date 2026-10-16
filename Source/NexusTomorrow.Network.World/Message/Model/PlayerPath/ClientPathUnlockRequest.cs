using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Client06BD, Direction.ClientToServer)]
public sealed partial class Client06BD
{
    [UInt(3)] public byte Unknown0 { get; private set; }
}
