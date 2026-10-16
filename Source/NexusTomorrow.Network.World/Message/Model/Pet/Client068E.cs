using NexusTomorrow.Game.Static.Pet;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Client068E, Direction.ClientToServer)]
public sealed partial class Client068E
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(5)] public byte Unknown1 { get; private set; }
}
