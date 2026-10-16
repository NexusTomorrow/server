using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Client068C, Direction.ClientToServer)]
public sealed partial class Client068C
{
    [UInt(2)] public byte Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
    [WideString] public string Unknown2 { get; private set; }
}
