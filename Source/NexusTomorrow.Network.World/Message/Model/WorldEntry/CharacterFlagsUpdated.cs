using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

[WireMessage(GameMessageOpcode.Server00FE, Direction.ServerToClient)]
public sealed partial class CharacterFlagsUpdated
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
