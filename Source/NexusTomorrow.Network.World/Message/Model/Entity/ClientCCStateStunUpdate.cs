using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

// May send this multiple times while the player is stunned.
[Message(GameMessageOpcode.ClientCCStateStunUpdate)]
[WireMessage(GameMessageOpcode.ClientCCStateStunUpdate, Direction.ClientToServer)]
public sealed partial class ClientCCStateStunUpdate : IReadable
{
    [UInt(8)] public CCStateStunVictimGameplay InputPressed { get; private set; }
    [UInt(8)] public CCStateStunVictimGameplay InputHeld { get; private set; }

    public void Read(GamePacketReader reader)
    {
        InputPressed = reader.ReadEnum<CCStateStunVictimGameplay>(8u); // The direction inputs considered 'key up'
        InputHeld = reader.ReadEnum<CCStateStunVictimGameplay>(8u); // The directions inputs considered 'key down'
    }
}
