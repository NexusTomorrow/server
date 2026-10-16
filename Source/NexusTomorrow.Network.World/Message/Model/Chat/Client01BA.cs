using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Client01BA, Direction.ClientToServer)]
public sealed partial class Client01BA
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
    [WideString] public string Unknown1 { get; private set; } = "";
    [WideString] public string Unknown2 { get; private set; } = "";
    [UInt(32)] public uint Unknown3 { get; private set; }
}
