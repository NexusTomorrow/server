using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

/// <summary>
/// The client's login request on the login connection, its answer to a valid <see cref="Network.Message.Model.ServerHello"/>: its build,
/// the account and the game token the STS issued, and a description of the client and its machine.
/// </summary>
/// <remarks>
/// The client sends it once per login connection, inside a <see cref="Network.Message.Model.ClientEnvelope"/>, only once it has an account
/// name, and then waits for <see cref="LoginAccepted"/> or <see cref="LoginDenied"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0592, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class LoginRequest
{
    /// <summary>
    /// A description of the client's machine.
    /// </summary>
    [WireStruct]
    public sealed partial class MachineInfo
    {
        /// <summary>
        /// The processor, as Windows' management interface describes it.
        /// </summary>
        [WireStruct]
        public sealed partial class ProcessorInfo
        {
            /// <summary>
            /// The processor's manufacturer.
            /// </summary>
            [WideString] public string Manufacturer { get; set; } = "";

            /// <summary>
            /// The processor's name.
            /// </summary>
            [WideString] public string Name { get; set; } = "";

            /// <summary>
            /// The processor's description.
            /// </summary>
            [WideString] public string Description { get; set; } = "";

            /// <summary>
            /// The processor's family code.
            /// </summary>
            [UInt(32)] public uint Family { get; set; }

            /// <summary>
            /// The processor's level.
            /// </summary>
            [UInt(32)] public uint Level { get; set; }

            /// <summary>
            /// The processor's revision; 0 when Windows gives none.
            /// </summary>
            [UInt(32)] public uint Revision { get; set; }

            /// <summary>
            /// The processor's maximum clock speed, in MHz.
            /// </summary>
            [UInt(32)] public uint ClockMhz { get; set; }

            /// <summary>
            /// The processor's core count; its logical processor count when Windows reports no cores.
            /// </summary>
            [UInt(32)] public uint Cores { get; set; }
        }

        /// <summary>
        /// The default display adapter, as Direct3D 9 identifies it.
        /// </summary>
        [WireStruct]
        public sealed partial class AdapterInfo
        {
            /// <summary>
            /// The adapter's description.
            /// </summary>
            [WideString] public string Description { get; set; } = "";

            /// <summary>
            /// The adapter's vendor id.
            /// </summary>
            [UInt(32)] public uint VendorId { get; set; }

            /// <summary>
            /// The adapter's device id.
            /// </summary>
            [UInt(32)] public uint DeviceId { get; set; }

            /// <summary>
            /// The adapter's subsystem id.
            /// </summary>
            [UInt(32)] public uint SubsystemId { get; set; }

            /// <summary>
            /// The adapter's revision.
            /// </summary>
            [UInt(32)] public uint Revision { get; set; }

            /// <summary>
            /// The adapter's memory, in MiB. Windows' own figure stops at 4095.
            /// </summary>
            [UInt(32)] public uint MemoryMb { get; set; }
        }

        /// <summary>
        /// The processor.
        /// </summary>
        public ProcessorInfo Processor { get; } = new();

        /// <summary>
        /// The machine's physical memory, in MiB, rounded up.
        /// </summary>
        [UInt(32)] public uint MemoryMb { get; set; }

        /// <summary>
        /// The default display adapter.
        /// </summary>
        public AdapterInfo Adapter { get; } = new();

        /// <summary>
        /// The processor architecture: 0x10000 plus a code, 1 for x86, 2 for x64, 3 for IA-64, 4 for ARM and 0 for any other.
        /// </summary>
        [UInt(32)] public uint Architecture { get; set; }

        /// <summary>
        /// The Windows version, as (major &lt;&lt; 16) | minor.
        /// </summary>
        [UInt(32)] public uint OsVersion { get; set; }

        /// <summary>
        /// The Windows service pack, as (major &lt;&lt; 16) | minor.
        /// </summary>
        [UInt(32)] public uint OsServicePack { get; set; }

        /// <summary>
        /// The Windows edition: the suite mask in bits 16 to 31, the product type in bits 8 to 15, and in bits 0 to 3 whether Windows is
        /// Server R2, Media Center, Starter and Tablet PC.
        /// </summary>
        [UInt(32)] public uint OsEdition { get; set; }
    }

    /// <summary>
    /// The client's build, 16042.
    /// </summary>
    [UInt(32)] public uint Build { get; set; }

    /// <summary>
    /// A fingerprint of the client's install: a fixed seed folded, byte by byte, over the SHA-1 digests of the running executable and of its
    /// patch index file. The same install sends the same value on every login.
    /// </summary>
    [UInt(64)] public ulong Fingerprint { get; set; }

    /// <summary>
    /// The account name the player logged in with, as typed. At most 251 characters.
    /// </summary>
    [TerminatedWideString] public string AccountName { get; set; } = "";

    /// <summary>
    /// The user id the STS returned when the login finished, parsed from its text form.
    /// </summary>
    public NetworkGuid StsUserId { get; } = new();

    /// <summary>
    /// The game token the STS issued for this login, parsed from its text form.
    /// </summary>
    [WireSecret] public NetworkGuid GameToken { get; } = new();

    /// <summary>
    /// The client's own IPv4 address on its STS connection, as its socket reports the local end, as a host-order integer: a.b.c.d is
    /// (a &lt;&lt; 24) | (b &lt;&lt; 16) | (c &lt;&lt; 8) | d.
    /// </summary>
    [UInt(32)] public uint ClientAddress { get; set; }

    /// <summary>
    /// The client's interface language, as an id in the client's Language table.
    /// </summary>
    [UInt(32)] public uint LanguageId { get; set; }

    /// <summary>
    /// The client's game mode, set when it starts: 0 by default, and any value of 2 or more becomes 0.
    /// </summary>
    [UInt(32)] public uint GameMode { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It always sends 0.
    /// </summary>
    [UInt(32)] public uint Unknown8 { get; set; }

    /// <summary>
    /// A description of the client's machine.
    /// </summary>
    public MachineInfo Machine { get; } = new();

    /// <summary>
    /// The realm data center the client was started for, as an id in the client's RealmDataCenter table.
    /// </summary>
    [UInt(32)] public uint RealmDataCenterId { get; set; }
}
