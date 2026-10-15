using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerLogoutUpdate)]
[WireMessage(GameMessageOpcode.ServerLogoutUpdate, Direction.ServerToClient)]
public sealed partial class ServerLogoutUpdate : IWritable
{
    [WireStruct]
    public sealed partial class SignatureBonuses : IWritable
    {
        [UInt(32)] public uint Xp { get; set; }
        [UInt(32)] public uint ElderPoints { get; set; }
        [FixedArray(15), UInt(64)] public ulong[] Currencies { get; set; }
        [FixedArray(19), UInt(64)] public ulong[] AccountCurrencies { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Xp);
            writer.Write(ElderPoints);
            writer.Write(Currencies, 15u);
            writer.Write(AccountCurrencies, 19u);
        }
    }

    [UInt(32)] public uint TimeTillLogout { get; set; }
    [UInt(1)] public bool CancelCamp { get; set; }
    public SignatureBonuses SignatureBonusData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TimeTillLogout);
        writer.Write(CancelCamp);
        SignatureBonusData.Write(writer);
    }
}
