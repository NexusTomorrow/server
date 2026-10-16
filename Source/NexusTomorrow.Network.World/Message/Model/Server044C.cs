using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server044C, Direction.ServerToClient)]
public sealed partial class Server044C
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    public Client00B5Part0 Unknown1 { get; set; } = new();

    [WideString] public string Unknown2 { get; set; } = "";
}
