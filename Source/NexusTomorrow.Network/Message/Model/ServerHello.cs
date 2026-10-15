using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

[Message(GameMessageOpcode.ServerHello)]
[WireMessage(GameMessageOpcode.ServerHello, Direction.ServerToClient)]
public sealed partial class ServerHello : IWritable
{
    [UInt(32)] public uint AuthVersion { get; set; }
    [UInt(32)] public uint RealmId { get; set; }
    [UInt(32)] public uint RealmGroupId { get; set; } // unused by client
    [UInt(32)] public uint RealmGroupEnum { get; set; } //  unused by client
    [UInt(64)] public ulong StartupTime { get; set; } //  unused by client
    [UInt(16)] public ushort ListenPort { get; set; } //  unused by client
    [UInt(5)] public byte ConnectionType { get; set; } // unused by client
    [UInt(32)] public uint AuthMessage { get; set; }
    [UInt(32)] public uint ProcessId { get; set; } //  unused by client
    [UInt(64)] public ulong ProcessCreationTime { get; set; } //  unused by client
    [UInt(32)] public uint Unused { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AuthVersion);
        writer.Write(RealmId);
        writer.Write(RealmGroupId);
        writer.Write(RealmGroupEnum);
        writer.Write(StartupTime);
        writer.Write(ListenPort);
        writer.Write(ConnectionType, 5);
        writer.Write(AuthMessage);
        writer.Write(ProcessId);
        writer.Write(ProcessCreationTime);
        writer.Write(Unused);
    }
}
