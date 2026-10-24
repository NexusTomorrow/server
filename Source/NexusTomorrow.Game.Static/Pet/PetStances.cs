namespace NexusTomorrow.Game.Static.Pet;

/// <summary>
/// A set of pet stances as the wire carries them: bit n is the stance <see cref="PetStance"/> n.
/// </summary>
[Flags]
public enum PetStances
{
    None = 0x00,
    Assist = 0x01,
    Stay = 0x02,
    Passive = 0x04,
    Defensive = 0x08,
    Aggressive = 0x10,
}
