using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell
{
    [Message(GameMessageOpcode.ClientSpellStopCast)]
    public class ClientSpellStopCast : IReadable
    {
        public uint ServerUniqueId { get; private set; } // first value of 0x7FD response, probably global increment
        public CastResult CastResult { get; private set; }
        public bool Cancelled { get; private set; } // true = cancelled, false = released

        public void Read(GamePacketReader reader)
        {
            ServerUniqueId = reader.ReadUInt();
            CastResult = reader.ReadEnum<CastResult>(9u);
            Cancelled   = reader.ReadBit();
        }
    }
}
