using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A pet's customization: which pet, its name and the flair in each of its four slots. The client keeps one for each pair of
/// <see cref="PetType"/> and <see cref="ObjectId"/>.
/// </summary>
[WireStruct]
public sealed partial class PetCustomization
{
    /// <summary>
    /// The kind of pet customized: a scanbot, a ground mount or a hoverboard.
    /// </summary>
    [UInt(2)] public PetType PetType { get; set; }

    /// <summary>
    /// The customized object, by id, which with <see cref="PetType"/> keys the customization. A scanbot's is its
    /// PathScientistScanBotProfile id.
    /// </summary>
    [UInt(32)] public uint ObjectId { get; set; }

    /// <summary>
    /// The pet's name, which the client shows as the scanbot's name. It keeps the first 31 characters.
    /// </summary>
    [WideString] public string Name { get; set; } = "";

    /// <summary>
    /// The flair in each of the pet's four slots, by PetFlair id, the index being the slot. The client keeps the low 16 bits of each.
    /// </summary>
    [FixedArray(4), UInt(32)] public uint[] PetFlairIds { get; set; } = new uint[4];
}
