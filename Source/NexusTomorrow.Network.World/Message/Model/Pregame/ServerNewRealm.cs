using NexusTomorrow.Game.Static.Pregame;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Client only processes message when on RealmSelect screen.
[Message(GameMessageOpcode.ServerNewRealm)]
[WireMessage(GameMessageOpcode.ServerNewRealm, Direction.ServerToClient)]
public sealed partial class ServerNewRealm : IWritable
{
    [WireStruct]
    public sealed partial class Gateway : IWritable
    {
        [UInt(32)] public uint Address { get; set; }
        [UInt(16)] public ushort Port { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Address);
            writer.Write(Port);
        }
    }

    [UInt(32)] public uint Unused { get; set; }
    [FixedArray(16), UInt(8)] public byte[] SessionKey { get; set; }
    public Gateway GatewayData { get; set; }
    [UInt(1)] public bool Unused2 { get; set; }
    [WideString] public string RealmName { get; set; }
    [UInt(32)] public RealmFlag Flags { get; set; }
    [UInt(2)] public RealmType Type { get; set; }
    [UInt(21)] public uint NoteTextId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unused);
        writer.WriteBytes(SessionKey, 16u);
        GatewayData.Write(writer);
        writer.Write(Unused2);
        writer.WriteStringWide(RealmName);
        writer.Write(Flags, 32u);
        writer.Write(Type, 2u);
        writer.Write(NoteTextId, 21u);
    }
}
