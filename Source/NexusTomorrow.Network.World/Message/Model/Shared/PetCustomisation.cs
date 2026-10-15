using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class PetCustomisation
{
    public const byte MaxCustomisationFlairs = 4;

    [UInt(2)] public PetType PetType { get; set; }
    [UInt(32)] public uint PetObjectId { get; set; }
    [WideString] public string PetName { get; set; }
    // there are 4 slots - not sure if uint or some short/short struct
    [FixedArray(MaxCustomisationFlairs), UInt(32)] public uint[] SlotFlairIds { get; set; } = new uint[MaxCustomisationFlairs];

    public PetCustomisation()
    {
        for (int i = 0; i < SlotFlairIds.Length; i++)
            SlotFlairIds[i] = 0;
    }
}
