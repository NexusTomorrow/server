using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
public sealed partial class Client0959Part0
{
    [UInt(4)] public byte Unknown0 { get; private set; }
    [Union(nameof(Unknown0))] public IClient0959Union0 Unknown1 { get; private set; } = new Client0959Union0Arm00();
}
