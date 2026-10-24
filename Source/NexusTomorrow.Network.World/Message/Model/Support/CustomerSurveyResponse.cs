using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[WireMessage(GameMessageOpcode.Client033F, Direction.ClientToServer)]
public sealed partial class CustomerSurveyResponse
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
    [Union(nameof(Unknown0))] public IClient033FUnion0 Unknown1 { get; private set; } = new Client033FUnion0Arm01();
    [WideString] public string Unknown2 { get; private set; } = "";
}
