using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Client0099, Direction.ClientToServer)]
public sealed partial class ExplorerSearchCast
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(2)] public byte Unknown1 { get; private set; }
    [UInt(14)] public ushort Unknown2 { get; private set; }
}
