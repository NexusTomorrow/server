using NexusTomorrow.Game.Static.Pregame;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[WireMessage(GameMessageOpcode.ServerRealmInfo, Direction.ServerToClient)]
public sealed partial class ServerRealmInfo
{
    [UInt(32)] public uint Address { get; set; }
    [UInt(16)] public ushort Port { get; set; }
    [FixedArray(16), UInt(8)] public byte[] SessionKey { get; set; }
    [UInt(32)] public uint AccountId { get; set; }
    [WideString] public string RealmName { get; set; }
    [UInt(32)] public RealmFlag Flags { get; set; }
    [UInt(2)] public RealmType Type { get; set; }
    [UInt(21)] public uint NoteTextId { get; set; }
}
