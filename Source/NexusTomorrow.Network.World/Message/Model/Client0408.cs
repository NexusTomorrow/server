using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client0408, Direction.ClientToServer)]
public sealed partial class Client0408
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    public Client00B5Part0 Unknown1 { get; set; } = new();

    [UInt(32)] public uint Unknown2 { get; set; }

    [UInt(32)] public uint Unknown3 { get; set; }
}
