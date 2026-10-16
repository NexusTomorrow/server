using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client04B1, Direction.ClientToServer)]
public partial class Client04B1
{
    [WireStruct]
    public sealed partial class Client04B1Part0
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
    }

    public Client00B5Part0 Unknown0 { get; private set; } = new();
    [UInt(32)] public uint Unknown1 { get; private set; }
    public Client04B1Part0 Unknown2 { get; private set; } = new();
    [WideString] public string Unknown3 { get; private set; }
    [UInt(6)] public byte Unknown4 { get; private set; }
}
