using NexusTomorrow.Game.Static.Setting;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerInstanceSettings)]
    public class ServerInstanceSettings : IWritable
    {
        [Flags]
        public enum WorldSetting
        {
            WorldForcesLevelScaling = 0x01,
            TransmatTeleport = 0x04, // Causes player to transmat recall if this is sent before OnPlayerWorld (0x61)
            HoloCryptTeleport = 0x08, // Causes holocrypt voice over if this is sent before OnPlayerWorld (0x61)
        }

        public WorldDifficulty Difficulty { get; set; }
        public uint PrimeLevel { get; set; }
        public WorldSetting Flags { get; set; }
        public uint ClientEntitySendUpdateInterval { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Difficulty, 2u);
            writer.Write(PrimeLevel);
            writer.Write(Flags, 8u);
            writer.Write(ClientEntitySendUpdateInterval);
        }
    }
}
