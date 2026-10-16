using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01D3, Direction.ServerToClient)]
public sealed partial class Server01D3
{
    public Server01B5Part0 Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
    [UInt(16)] public ushort Unknown2 { get; set; }
}
