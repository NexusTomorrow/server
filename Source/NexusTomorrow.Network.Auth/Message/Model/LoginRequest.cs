using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

/// <summary>
/// The client's login request on the login connection, its answer to a valid <see cref="Network.Message.Model.ServerHello"/>.
/// </summary>
/// <remarks>
/// The client sends it only once it has an account name, and then waits for <see cref="LoginAccepted"/> or <see cref="LoginDenied"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0592, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class LoginRequest
{
    [WireStruct]
    public sealed partial class Client0592Part0
    {
        [WireStruct]
        public sealed partial class Client0592Part1
        {
            [WideString] public string Unknown0 { get; private set; } = "";
            [WideString] public string Unknown1 { get; private set; } = "";
            [WideString] public string Unknown2 { get; private set; } = "";
            [UInt(32)] public uint Unknown3 { get; private set; }
            [UInt(32)] public uint Unknown4 { get; private set; }
            [UInt(32)] public uint Unknown5 { get; private set; }
            [UInt(32)] public uint Unknown6 { get; private set; }
            [UInt(32)] public uint Unknown7 { get; private set; }
        }

        [WireStruct]
        public sealed partial class Client0592Part2
        {
            [WideString] public string Unknown0 { get; private set; } = "";
            [UInt(32)] public uint Unknown1 { get; private set; }
            [UInt(32)] public uint Unknown2 { get; private set; }
            [UInt(32)] public uint Unknown3 { get; private set; }
            [UInt(32)] public uint Unknown4 { get; private set; }
            [UInt(32)] public uint Unknown5 { get; private set; }
        }

        public Client0592Part1 Unknown0 { get; } = new();
        [UInt(32)] public uint Unknown1 { get; private set; }
        public Client0592Part2 Unknown2 { get; } = new();
        [UInt(32)] public uint Unknown3 { get; private set; }
        [UInt(32)] public uint Unknown4 { get; private set; }
        [UInt(32)] public uint Unknown5 { get; private set; }
        [UInt(32)] public uint Unknown6 { get; private set; }
    }

    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [TerminatedWideString] public string Unknown2 { get; private set; } = "";
    public NetworkGuid Unknown3 { get; } = new();
    public NetworkGuid Unknown4 { get; } = new();
    [UInt(32)] public uint Unknown5 { get; private set; }
    [UInt(32)] public uint Unknown6 { get; private set; }
    [UInt(32)] public uint Unknown7 { get; private set; }
    [UInt(32)] public uint Unknown8 { get; private set; }
    public Client0592Part0 Unknown9 { get; } = new();
    [UInt(32)] public uint Unknown10 { get; private set; }
}
