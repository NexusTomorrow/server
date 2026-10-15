using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientStatisticsGfx, Direction.ClientToServer)]
public sealed partial class ClientStatisticsGfx
{
    [Float32] public float GfxStat1 { get; private set; }
    [Float32] public float GfxStat2 { get; private set; }
    [Float32] public float GfxStat3 { get; private set; }
    [Float32] public float GfxStat4 { get; private set; }
    [Float32] public float GfxStat5 { get; private set; }
    [Float32] public float GfxStat6 { get; private set; }
    [Float32] public float GfxStat7 { get; private set; }
    [Float32] public float GfxStat8 { get; private set; }
}
