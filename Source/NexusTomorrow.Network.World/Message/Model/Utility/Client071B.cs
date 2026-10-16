using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Client071B, Direction.ClientToServer)]
public sealed partial class Client071B
{
    public Client00B5Part0 Unknown0 { get; } = new();
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    [UInt(32)] public uint Unknown3 { get; private set; }
}
