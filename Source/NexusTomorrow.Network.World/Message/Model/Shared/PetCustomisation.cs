using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server012EPart0
{
    public const byte MaxCustomisationFlairs = 4;

    [UInt(2)] public byte Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; }
    [FixedArray(MaxCustomisationFlairs), UInt(32)] public uint[] Unknown3 { get; set; } = new uint[MaxCustomisationFlairs];

    public Server012EPart0()
    {
        for (int i = 0; i < Unknown3.Length; i++)
            Unknown3[i] = 0;
    }
}
