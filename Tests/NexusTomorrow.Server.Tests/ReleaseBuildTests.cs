#if !DEBUG
namespace NexusTomorrow.Server.Tests;

// Compiled into Release builds only, where the workflow runs it: in-process tooling never ships.
public sealed class ReleaseBuildTests
{
    [Fact]
    public void AReleaseServerDoesNotReferenceTheTools()
    {
        Assert.DoesNotContain(typeof(CoreServices).Assembly.GetReferencedAssemblies(), name => name.Name == "NexusTomorrow.Tools");
    }
}
#endif
