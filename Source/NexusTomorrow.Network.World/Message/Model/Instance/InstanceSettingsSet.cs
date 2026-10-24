using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.Client0163, Direction.ClientToServer)]
public sealed partial class InstanceSettingsSet
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(2)] public byte Unknown1 { get; private set; }
    [UInt(8)] public byte Unknown2 { get; private set; }
    [UInt(11)] public ushort Unknown3 { get; private set; }
}
