using NexusTomorrow.Game.Static;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[Message(GameMessageOpcode.ClientHelloAuth)]
[WireMessage(GameMessageOpcode.ClientHelloAuth, Direction.ClientToServer)]
public sealed partial class ClientHelloAuth : IReadable
{
    [WireStruct]
    public sealed partial class HardwareInformation : IReadable
    {
        [WireStruct]
        public sealed partial class CpuInformation : IReadable
        {
            [WideString] public string Manufacturer { get; private set; }
            [WideString] public string Name { get; private set; }
            [WideString] public string Description { get; private set; }
            [UInt(32)] public uint Family { get; private set; }
            [UInt(32)] public uint Level { get; private set; }
            [UInt(32)] public uint Revision { get; private set; }
            [UInt(32)] public uint MaxClockSpeed { get; private set; }
            [UInt(32)] public uint NumberOfCores { get; private set; }

            public void Read(GamePacketReader reader)
            {
                Manufacturer = reader.ReadWideString();
                Name = reader.ReadWideString();
                Description = reader.ReadWideString();
                Family = reader.ReadUInt();
                Level = reader.ReadUInt();
                Revision = reader.ReadUInt();
                MaxClockSpeed = reader.ReadUInt();
                NumberOfCores = reader.ReadUInt();
            }
        }

        [WireStruct]
        public sealed partial class GpuInformation : IReadable
        {
            [WideString] public string Name { get; private set; }
            [UInt(32)] public uint VendorId { get; private set; }
            [UInt(32)] public uint DeviceId { get; private set; }
            [UInt(32)] public uint SubSysId { get; private set; }
            [UInt(32)] public uint Revision { get; private set; }
            [UInt(32)] public uint AdapterRamMB { get; private set; }

            public void Read(GamePacketReader reader)
            {
                Name = reader.ReadWideString();
                VendorId = reader.ReadUInt();
                DeviceId = reader.ReadUInt();
                SubSysId = reader.ReadUInt();
                Revision = reader.ReadUInt();
                AdapterRamMB = reader.ReadUInt();
            }
        }

        public CpuInformation Cpu { get; } = new();
        [UInt(32)] public uint MemoryPhysical { get; private set; }
        public GpuInformation Gpu { get; } = new();
        [UInt(32)] public uint Architecture { get; private set; }
        [UInt(32)] public uint OsVersion { get; private set; }
        [UInt(32)] public uint ServicePack { get; private set; }
        [UInt(32)] public uint ProductType { get; private set; }

        public void Read(GamePacketReader reader)
        {
            Cpu.Read(reader);
            MemoryPhysical = reader.ReadUInt();
            Gpu.Read(reader);

            Architecture = reader.ReadUInt(); // v3 | 0x10000
            OsVersion = reader.ReadUInt(); // (dwMajorVersion << 16) | dwMinorVersion
            ServicePack = reader.ReadUInt(); // (wServicePackMajor << 16) | wServicePackMinor
            ProductType = reader.ReadUInt();
        }
    }

    [UInt(32)] public uint Build { get; private set; }
    [UInt(64)] public ulong CryptKeyInteger { get; private set; }
    [TerminatedWideString] public string AccountString { get; private set; }
    public NetworkGuid UUID_1 { get; } = new();
    public NetworkGuid GameToken { get; } = new();
    [UInt(32)] public uint InetAddress { get; private set; }
    [UInt(32)] public Language Language { get; private set; }
    [UInt(32)] public uint GameMode { get; private set; }
    [UInt(32)] public uint Unused { get; private set; }
    public HardwareInformation Hardware { get; } = new();
    [UInt(32)] public uint RealmDataCenterId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Build = reader.ReadUInt();
        CryptKeyInteger = reader.ReadULong(); // 0x1588
        AccountString = reader.ReadWideStringFixed();

        UUID_1.Read(reader);
        GameToken.Read(reader);

        InetAddress = reader.ReadUInt();
        Language = reader.ReadEnum<Language>(32u);
        GameMode = reader.ReadUInt();
        Unused = reader.ReadUInt();

        Hardware.Read(reader);

        RealmDataCenterId = reader.ReadUInt();
    }
}
