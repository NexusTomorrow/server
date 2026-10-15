using NexusTomorrow.Game.Static.Setting;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance
{
    [Message(GameMessageOpcode.ClientSetInstanceSettings)]
    public class ClientSetInstanceSettings : IReadable
    {
        public uint InstancePortalUnitId { get; private set; }
        public WorldDifficulty Difficulty { get; private set; }
        public byte PrimeLevel { get; private set; }
        public ushort Rally { get; private set; } // Was probably intended for more flags but is only used for one setting.

        public void Read(GamePacketReader reader)
        {
            InstancePortalUnitId = reader.ReadUInt();
            Difficulty = reader.ReadEnum<WorldDifficulty>(2u);
            PrimeLevel = reader.ReadByte();
            Rally = reader.ReadUShort(11u);
        }
    }
}
