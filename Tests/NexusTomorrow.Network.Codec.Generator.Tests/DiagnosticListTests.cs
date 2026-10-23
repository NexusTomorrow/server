using System.Reflection;
using Microsoft.CodeAnalysis;

namespace NexusTomorrow.Network.Codec.Generator.Tests;

public sealed class DiagnosticListTests
{
    [Fact]
    public void TheListHoldsEveryDeclaredDescriptorInDeclarationOrder()
    {
        string[] declared = [.. typeof(Diagnostics)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(DiagnosticDescriptor))
            .OrderBy(field => field.MetadataToken)
            .Select(field => ((DiagnosticDescriptor)field.GetValue(null)!).Id)];

        string[] listed = [.. Diagnostics.All.Select(descriptor => descriptor.Id)];

        Assert.NotEmpty(declared);
        Assert.Equal(declared, listed);
    }

    [Fact]
    public void EveryListedDescriptorIsFoundByItsId()
    {
        foreach (DiagnosticDescriptor descriptor in Diagnostics.All)
            Assert.Same(descriptor, Diagnostics.ById(descriptor.Id));
    }
}
