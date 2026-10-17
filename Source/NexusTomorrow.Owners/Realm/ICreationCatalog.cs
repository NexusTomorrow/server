namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// The character creation options by id.
/// </summary>
public interface ICreationCatalog
{
    /// <summary>
    /// The option with <paramref name="creationId"/>, or null when there is none.
    /// </summary>
    CreationOption? Find(uint creationId);
}
