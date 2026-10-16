namespace NexusTomorrow.Network.World.Message.Model.Shared;

[Flags]
public enum UpdateHealthMask
{
    None = 0x0000,
    FallDamage = 0x0040,
    SuffocateDamage = 0x0200
}
