using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client0471, Direction.ClientToServer)]
public sealed partial class Client0471
{
    public Client00B5Part0 Unknown0 { get; private set; } = new Client00B5Part0();
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [UInt(64)] public ulong Unknown2 { get; private set; }
    public Server0111Part2 Unknown3 { get; private set; } = new Server0111Part2();
    public Server0111Part2 Unknown4 { get; private set; } = new Server0111Part2();
    [UInt(32)] public uint Unknown5 { get; private set; }
}
