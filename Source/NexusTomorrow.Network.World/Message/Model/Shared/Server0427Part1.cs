using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0427Part1
{
    [Array(CountBits = 32)] public Server0427Part2[] Unknown0 { get; set; }
}
