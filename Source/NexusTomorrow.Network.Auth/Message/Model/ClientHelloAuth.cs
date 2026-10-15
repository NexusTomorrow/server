using NexusTomorrow.Game.Static;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[WireMessage(GameMessageOpcode.ClientHelloAuth, Direction.ClientToServer)]
public sealed partial class ClientHelloAuth
{
    [WireStruct]
    public sealed partial class HardwareInformation
    {
        [WireStruct]
        public sealed partial class CpuInformation
        {
            [WideString] public string Manufacturer { get; private set; }
            [WideString] public string Name { get; private set; }
            [WideString] public string Description { get; private set; }
            [UInt(32)] public uint Family { get; private set; }
            [UInt(32)] public uint Level { get; private set; }
            [UInt(32)] public uint Revision { get; private set; }
            [UInt(32)] public uint MaxClockSpeed { get; private set; }
            [UInt(32)] public uint NumberOfCores { get; private set; }
        }

        [WireStruct]
        public sealed partial class GpuInformation
        {
            [WideString] public string Name { get; private set; }
            [UInt(32)] public uint VendorId { get; private set; }
            [UInt(32)] public uint DeviceId { get; private set; }
            [UInt(32)] public uint SubSysId { get; private set; }
            [UInt(32)] public uint Revision { get; private set; }
            [UInt(32)] public uint AdapterRamMB { get; private set; }
        }

        public CpuInformation Cpu { get; } = new();
        [UInt(32)] public uint MemoryPhysical { get; private set; }
        public GpuInformation Gpu { get; } = new();
        [UInt(32)] public uint Architecture { get; private set; }
        [UInt(32)] public uint OsVersion { get; private set; }
        [UInt(32)] public uint ServicePack { get; private set; }
        [UInt(32)] public uint ProductType { get; private set; }
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
}
