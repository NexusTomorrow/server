using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
public sealed partial class WhoParameterClass : IWhoParameterData
{
    [UInt(8)] public Class ClassId { get; private set; }
    [Pad(6)] public Pad ClassIdHigh { get; }

    public void Read(GamePacketReader reader)
    {
        ClassId = reader.ReadEnum<Class>(14u);
    }
}
