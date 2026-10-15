using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[Message(GameMessageOpcode.ClientSetInputKeySet)]
[WireMessage(GameMessageOpcode.ClientSetInputKeySet, Direction.ClientToServer)]
public sealed partial class ClientSetInputKeySet : IReadable
{
    [UInt(32)] public InputSets InputKeySetEnum { get; private set; }

    public void Read(GamePacketReader reader)
    {
        InputKeySetEnum = reader.ReadEnum<InputSets>(32u);
    }
}
