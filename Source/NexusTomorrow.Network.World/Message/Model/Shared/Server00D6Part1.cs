using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server00D6Part1
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    public Server00D6Part0 Unknown1 { get; set; }
}
