using System.Reflection;
using System.Runtime.CompilerServices;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// Reads and writes a property or field through reflection, whatever its accessibility; a get-only auto-property is written through its backing field.
/// </summary>
internal static class MemberAccess
{
    internal const BindingFlags DeclaredInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    private const BindingFlags DeclaredPublicInstance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;

    public static Type TypeOf(MemberInfo member)
    {
        return member switch
        {
            PropertyInfo property => property.PropertyType,
            FieldInfo field => field.FieldType,
            _ => throw new ArgumentException($"{member.Name} is not a property or a field.", nameof(member)),
        };
    }

    public static object? GetValue(MemberInfo member, object target)
    {
        return member switch
        {
            PropertyInfo property => property.GetValue(target),
            FieldInfo field => field.GetValue(target),
            _ => throw new ArgumentException($"{member.Name} is not a property or a field.", nameof(member)),
        };
    }

    /// <summary>
    /// True for a field, and for a property with a setter of any accessibility, init-only included.
    /// </summary>
    public static bool HasSetter(MemberInfo member)
    {
        return member is FieldInfo || (member is PropertyInfo property && property.GetSetMethod(nonPublic: true) is not null);
    }

    /// <summary>
    /// Assign through the setter, else through a get-only auto-property's backing field; false when the member is computed and holds nothing to assign.
    /// </summary>
    public static bool TrySetValue(MemberInfo member, object target, object? value)
    {
        switch (member)
        {
            case FieldInfo field:
                field.SetValue(target, value);
                return true;
            case PropertyInfo property when property.GetSetMethod(nonPublic: true) is { } setter:
                setter.Invoke(target, [value]);
                return true;
            case PropertyInfo property when BackingFieldOf(property) is { } backingField:
                backingField.SetValue(target, value);
                return true;
            default:
                return false;
        }
    }

    public static void SetValue(MemberInfo member, object target, object? value)
    {
        if (!TrySetValue(member, target, value))
            throw new InvalidOperationException($"{member.DeclaringType?.Name}.{member.Name} has no setter and no backing field.");
    }

    /// <summary>
    /// The property, or failing that the public field, named <paramref name="name"/> on <paramref name="type"/> or the nearest base type that declares one.
    /// Fields are public only, so a backing field or an injected service is never taken for a twin.
    /// </summary>
    public static MemberInfo? Find(Type type, string name)
    {
        for (Type? current = type; current is not null && current != typeof(object); current = current.BaseType)
        {
            PropertyInfo? property = Array.Find(current.GetProperties(DeclaredInstance), candidate => candidate.Name == name && candidate.GetIndexParameters().Length == 0);
            if (property is not null)
                return property;
            if (current.GetField(name, DeclaredPublicInstance) is { } field)
                return field;
        }

        return null;
    }

    private static FieldInfo? BackingFieldOf(PropertyInfo property)
    {
        FieldInfo? field = property.DeclaringType?.GetField($"<{property.Name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        return field is not null && field.IsDefined(typeof(CompilerGeneratedAttribute)) ? field : null;
    }
}
