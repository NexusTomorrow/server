using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientStorefrontPurchaseCharacter)]
[WireMessage(GameMessageOpcode.ClientStorefrontPurchaseCharacter, Direction.ClientToServer)]
public sealed partial class ClientStorefrontPurchaseCharacter : IReadable
{
    [UInt(20)] public uint OfferId { get; set; } // OfferId
    [UInt(5)] public byte Unknown1 { get; set; }
    [UInt(14)] public ushort CurrencyId { get; set; } // CurrencyId - 1408 == NCCoin, 768 == Omnibits
    [UInt(20)] public uint Unknown3 { get; set; }
    public Identity Target { get; set; } = new();
    [UInt(20)] public uint Unknown6 { get; set; }

    public void Read(GamePacketReader reader)
    {
        OfferId = reader.ReadUInt(20);
        Unknown1 = reader.ReadByte(5);
        CurrencyId = reader.ReadUShort(14);
        Unknown3 = reader.ReadUInt(20);
        Target.Read(reader);
        Unknown6 = reader.ReadUInt(20);
    }
}
