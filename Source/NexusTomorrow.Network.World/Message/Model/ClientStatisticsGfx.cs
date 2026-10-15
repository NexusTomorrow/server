using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientStatisticsGfx)]
[WireMessage(GameMessageOpcode.ClientStatisticsGfx, Direction.ClientToServer)]
public sealed partial class ClientStatisticsGfx : IReadable
{
    [Float32] public float GfxStat1 { get; private set; }
    [Float32] public float GfxStat2 { get; private set; }
    [Float32] public float GfxStat3 { get; private set; }
    [Float32] public float GfxStat4 { get; private set; }
    [Float32] public float GfxStat5 { get; private set; }
    [Float32] public float GfxStat6 { get; private set; }
    [Float32] public float GfxStat7 { get; private set; }
    [Float32] public float GfxStat8 { get; private set; }

    public void Read(GamePacketReader reader)
    {
        GfxStat1 = reader.ReadSingle();
        GfxStat2 = reader.ReadSingle();
        GfxStat3 = reader.ReadSingle();
        GfxStat4 = reader.ReadSingle();
        GfxStat5 = reader.ReadSingle();
        GfxStat6 = reader.ReadSingle();
        GfxStat7 = reader.ReadSingle();
        GfxStat8 = reader.ReadSingle();
    }
}
