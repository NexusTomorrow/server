using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server06BE, Direction.ServerToClient)]
public sealed partial class Server06BE
{
    [UInt(8)] public byte Unknown0 { get; set; }
    [UInt(4)] public byte Unknown1 { get; set; }
}
