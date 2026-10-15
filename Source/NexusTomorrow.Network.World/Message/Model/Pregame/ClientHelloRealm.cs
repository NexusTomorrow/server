using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ClientHelloRealm)]
[WireMessage(GameMessageOpcode.ClientHelloRealm, Direction.ClientToServer)]
public sealed partial class ClientHelloRealm : IReadable
{
    [UInt(32)] public uint AccountId { get; private set; }
    [FixedArray(16), UInt(8)] public byte[] SessionKey { get; private set; }
    [UInt(64)] public ulong Unused { get; private set; }
    [WideString] public string AccountString { get; private set; }
    [UInt(32)] public uint Always3 { get; private set; }

    public void Read(GamePacketReader reader)
    {
        AccountId = reader.ReadUInt();
        SessionKey = reader.ReadBytes(16);
        Unused = reader.ReadULong();
        AccountString = reader.ReadWideString();
        Always3 = reader.ReadUInt();
    }
}
