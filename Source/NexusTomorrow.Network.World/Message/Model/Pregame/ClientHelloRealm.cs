using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.ClientHelloRealm, Direction.ClientToServer)]
public sealed partial class ClientHelloRealm
{
    [UInt(32)] public uint AccountId { get; private set; }
    [FixedArray(16), UInt(8)] public byte[] SessionKey { get; private set; }
    [UInt(64)] public ulong Unused { get; private set; }
    [WideString] public string AccountString { get; private set; }
    [UInt(32)] public uint Always3 { get; private set; }
}
