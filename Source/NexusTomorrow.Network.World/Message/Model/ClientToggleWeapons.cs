using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientToggleWeapons)]
[WireMessage(GameMessageOpcode.ClientToggleWeapons, Direction.ClientToServer)]
public sealed partial class ClientToggleWeapons : IReadable
{
    [UInt(1)] public bool ToggleState { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ToggleState = reader.ReadBit();
    }
}
