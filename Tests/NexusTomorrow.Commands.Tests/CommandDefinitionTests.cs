namespace NexusTomorrow.Commands.Tests;

public sealed class CommandDefinitionTests
{
    private static readonly CommandDefinition s_create = new()
    {
        Name = "account create",
        Description = "Create an account.",
        Permission = PermissionKeys.AccountCreate,
        Parameters = [new("email", "the login name"), new("password", "the password", Secret: true), new("note", "a note", Optional: true, Rest: true)],
        ExecuteAsync = (_, _) => throw new InvalidOperationException("Never run."),
    };

    [Fact]
    public void UsageShowsRequiredOptionalAndRestParameters()
    {
        Assert.Equal("account create <email> <password> [note...]", s_create.Usage());
    }

    [Fact]
    public void UsageMarksASecretParameterOnlyWhenAsked()
    {
        Assert.Equal("account create <email> <password, secret> [note...]", s_create.Usage(markSecrets: true));
    }
}
