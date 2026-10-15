using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientSpellStopCast)]
[WireMessage(GameMessageOpcode.ClientSpellStopCast, Direction.ClientToServer)]
public sealed partial class ClientSpellStopCast : IReadable
{
    [UInt(32)] public uint ServerUniqueId { get; private set; } // first value of 0x7FD response, probably global increment
    [UInt(9)] public CastResult CastResult { get; private set; }
    [UInt(1)] public bool Cancelled { get; private set; } // true = cancelled, false = released

    public void Read(GamePacketReader reader)
    {
        ServerUniqueId = reader.ReadUInt();
        CastResult = reader.ReadEnum<CastResult>(9u);
        Cancelled = reader.ReadBit();
    }
}
