namespace NexusTomorrow.ModelConverter;

/// <summary>
/// One member of a converted declaration, in wire order: an existing property or field with the attributes it gains,
/// or a member the converter adds, a shared count or a pad, which has an <see cref="AddedType"/>.
/// </summary>
/// <param name="Name">The member's name.</param>
/// <param name="Attributes">The attributes inside one list, without brackets; empty for a nested [WireStruct] member.</param>
/// <param name="AddedType">The C# type of an added member, or null for an existing one.</param>
internal sealed record WireMember(string Name, string Attributes, string? AddedType)
{
    public bool IsAdded => AddedType is not null;

    /// <summary>
    /// The declaration of an added member.
    /// </summary>
    public string Declaration => AddedType == "Pad"
        ? $"[{Attributes}] public Pad {Name} {{ get; }}"
        : $"[{Attributes}] public {AddedType} {Name} {{ get; set; }}";
}
