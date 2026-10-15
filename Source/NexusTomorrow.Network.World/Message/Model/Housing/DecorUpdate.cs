using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireStruct]
public sealed partial class DecorUpdate : IReadable
{
    public DecorInfo DecorInfo { get; internal set; } = new();
    [NotOnWire] public bool UseServiceToken { get; internal set; }

    public void Read(GamePacketReader reader)
    {
        DecorInfo.Read(reader);
    }
}
