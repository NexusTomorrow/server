using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0938, Direction.ServerToClient)]
public sealed partial class Server0938
{
    [WireStruct]
    public sealed partial class Server0938Part0
    {
        [UInt(5)] public byte Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    public Server0938Part0 Unknown1 { get; set; } = new();
}
