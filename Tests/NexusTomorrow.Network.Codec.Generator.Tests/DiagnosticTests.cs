using System.Globalization;
using Microsoft.CodeAnalysis;

namespace NexusTomorrow.Network.Codec.Generator.Tests;

public sealed class DiagnosticTests
{
    [Fact]
    public void MemberWithNoWireAttributeRaisesNtc001AndGetsNoSource()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                public int Value { get; set; }
            }
            """);

        AssertSingle(run, "NTC001", DiagnosticSeverity.Error, "int Value");
        Assert.DoesNotContain("Tests.Sample.g.cs", run.GeneratedSources);
    }

    [Fact]
    public void MemberWithAWidthRaisesNothing()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [UInt(32)] public uint Value { get; set; }
            }
            """);

        AssertClean(run);
        Assert.Contains("Tests.Sample.g.cs", run.GeneratedSources);
    }

    [Theory]
    [InlineData("byte", "UInt(9)")]
    [InlineData("bool", "UInt(2)")]
    [InlineData("long", "SInt(65)")]
    public void WidthTheTypeCannotHoldRaisesNtc002(string type, string attribute)
    {
        GeneratorRun run = GeneratorHarness.Run($$"""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [{{attribute}}] public {{type}} Value { get; set; }
            }
            """);

        AssertSingle(run, "NTC002", DiagnosticSeverity.Error, type + " Value");
    }

    [Fact]
    public void EnumValueWiderThanTheWidthRaisesNtc003()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public enum Mode : byte
            {
                Idle = 0,
                Eight = 8,
            }

            [WireStruct]
            public partial class Sample
            {
                [UInt(3)] public Mode Current { get; set; }
            }
            """);

        Diagnostic diagnostic = AssertSingle(run, "NTC003", DiagnosticSeverity.Error, "Mode Current");
        Assert.Contains("Mode.Eight = 8", MessageOf(diagnostic));
    }

    [Fact]
    public void EnumValuesThatFitTheWidthRaiseNothing()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public enum Mode : byte
            {
                Idle = 0,
                Eight = 8,
            }

            [WireStruct]
            public partial class Sample
            {
                [UInt(4)] public Mode Current { get; set; }
            }
            """);

        AssertClean(run);
    }

    [Fact]
    public void CountFromNamingALaterMemberRaisesNtc004()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using System.Collections.Generic;
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [Array(CountFrom = nameof(Length)), UInt(8)] public List<byte> Values { get; set; } = [];

                [Count, UInt(8)] public byte Length { get; set; }
            }
            """);

        Diagnostic diagnostic = AssertSingle(run, "NTC004", DiagnosticSeverity.Error, "List<byte> Values");
        Assert.Contains("is declared after it", MessageOf(diagnostic));
    }

    [Fact]
    public void CountFromNamingAMemberWithoutCountRaisesNtc004()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using System.Collections.Generic;
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [UInt(8)] public byte Length { get; set; }

                [Array(CountFrom = nameof(Length)), UInt(8)] public List<byte> Values { get; set; } = [];
            }
            """);

        Diagnostic diagnostic = AssertSingle(run, "NTC004", DiagnosticSeverity.Error, "List<byte> Values");
        Assert.Contains("is not a [Count] member", MessageOf(diagnostic));
    }

    [Fact]
    public void CountNoArrayNamesRaisesNtc009()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [Count, UInt(8)] public byte Length { get; set; }
            }
            """);

        Diagnostic diagnostic = AssertSingle(run, "NTC009", DiagnosticSeverity.Error, "byte Length");
        Assert.Contains("no [Array] names this [Count] member", MessageOf(diagnostic));
    }

    [Fact]
    public void UnionSelectorNamingAMissingMemberRaisesNtc004()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public interface IArm : IWireStruct
            {
            }

            [WireStruct]
            [UnionCase(0)]
            public sealed partial class Zero : IArm
            {
            }

            [WireStruct]
            public partial class Holder
            {
                [Union("Missing")] public IArm Body { get; set; } = new Zero();
            }
            """);

        Diagnostic diagnostic = AssertSingle(run, "NTC004", DiagnosticSeverity.Error, "IArm Body");
        Assert.Contains("is not a wire member of this type", MessageOf(diagnostic));
        Assert.DoesNotContain("Tests.Holder.g.cs", run.GeneratedSources);
    }

    [Fact]
    public void TwoMessagesWithOneOpcodeAndDirectionRaiseNtc005OnTheLaterName()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;
            using NexusTomorrow.Network.Message;

            namespace Tests;

            [WireMessage(GameMessageOpcode.ServerHello, Direction.ServerToClient)]
            public sealed partial class Alpha
            {
            }

            [WireMessage(GameMessageOpcode.ServerHello, Direction.ServerToClient)]
            public sealed partial class Beta
            {
            }
            """);

        AssertSingle(run, "NTC005", DiagnosticSeverity.Error, "class Beta");
        string registry = Assert.Contains("WireMessages.g.cs", run.GeneratedSources);
        Assert.Contains("global::Tests.Alpha", registry);
        Assert.DoesNotContain("Beta", registry);
    }

    [Fact]
    public void OneOpcodeInOppositeDirectionsRaisesNothing()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;
            using NexusTomorrow.Network.Message;

            namespace Tests;

            [WireMessage(GameMessageOpcode.ServerHello, Direction.ServerToClient)]
            public sealed partial class Alpha
            {
            }

            [WireMessage(GameMessageOpcode.ServerHello, Direction.ClientToServer)]
            public sealed partial class Beta
            {
            }
            """);

        AssertClean(run);
        string registry = Assert.Contains("WireMessages.g.cs", run.GeneratedSources);
        Assert.Contains("global::Tests.Alpha", registry);
        Assert.Contains("global::Tests.Beta", registry);
    }

    [Fact]
    public void TwoUnionArmsWithOneValueRaiseNtc005AndTheirHolderGetsNoSource()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public interface IArm : IWireStruct
            {
            }

            [WireStruct]
            [UnionCase(1)]
            public sealed partial class Left : IArm
            {
            }

            [WireStruct]
            [UnionCase(1)]
            public sealed partial class Right : IArm
            {
            }

            [WireStruct]
            public partial class Holder
            {
                [UInt(8)] public byte Selector { get; set; }

                [Union(nameof(Selector))] public IArm Body { get; set; } = new Left();
            }
            """);

        AssertSingle(run, "NTC005", DiagnosticSeverity.Error, "class Right");
        Assert.DoesNotContain("Tests.Holder.g.cs", run.GeneratedSources);
    }

    [Fact]
    public void UIntNarrowerThanASignedTypeWarnsWithNtc006AndStillGenerates()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [UInt(14)] public int Value { get; set; }
            }
            """);

        AssertSingle(run, "NTC006", DiagnosticSeverity.Warning, "int Value");
        Assert.Contains("Tests.Sample.g.cs", run.GeneratedSources);
        Assert.Empty(run.CompilationErrors);
    }

    [Fact]
    public void SIntOnASignedTypeRaisesNothing()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [SInt(14)] public int Value { get; set; }
            }
            """);

        AssertClean(run);
    }

    // NTC003 proves every member of an enum fits its unsigned width, so a signed underlying type (int by
    // default) gives NTC006 nothing to warn about; under TreatWarningsAsErrors a warning here fails the build.
    [Fact]
    public void UIntOnAnIntBackedEnumWhoseValuesFitRaisesNothing()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public enum Mode
            {
                Idle,
                Busy,
            }

            [WireStruct]
            public partial class Sample
            {
                [UInt(2)] public Mode Current { get; set; }
            }
            """);

        AssertClean(run);
    }

    [Fact]
    public void WireMembersInTwoPartialDeclarationsRaiseNtc007()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [UInt(8)] public byte First { get; set; }
            }

            partial class Sample
            {
                [UInt(8)] public byte Second { get; set; }
            }
            """);

        AssertSingle(run, "NTC007", DiagnosticSeverity.Error, "public partial class Sample");
    }

    [Fact]
    public void NonPartialClassRaisesNtc008()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public class Sample
            {
                [UInt(8)] public byte Value { get; set; }
            }
            """);

        AssertSingle(run, "NTC008", DiagnosticSeverity.Error, "public class Sample");
        Assert.DoesNotContain("Tests.Sample.g.cs", run.GeneratedSources);
    }

    [Fact]
    public void PartialClassInANonPartialClassRaisesNtc008()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public class Outer
            {
                [WireStruct]
                public partial class Inner
                {
                    [UInt(8)] public byte Value { get; set; }
                }
            }
            """);

        AssertSingle(run, "NTC008", DiagnosticSeverity.Error, "class Inner");
    }

    [Fact]
    public void PartialClassInAPartialClassRaisesNothing()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public partial class Outer
            {
                [WireStruct]
                public partial class Inner
                {
                    [UInt(8)] public byte Value { get; set; }
                }
            }
            """);

        AssertClean(run);
        Assert.Contains("Tests.Outer.Inner.g.cs", run.GeneratedSources);
    }

    [Fact]
    public void ArrayWithBothCountBitsAndCountFromRaisesNtc009()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using System.Collections.Generic;
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [UInt(8)] public byte Length { get; set; }

                [Array(CountBits = 8, CountFrom = nameof(Length)), UInt(8)] public List<byte> Values { get; set; } = [];
            }
            """);

        Diagnostic diagnostic = AssertSingle(run, "NTC009", DiagnosticSeverity.Error, "List<byte> Values");
        Assert.Contains("exactly one of CountBits and CountFrom", MessageOf(diagnostic));
    }

    [Fact]
    public void Float32OnAnIntRaisesNtc009()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [Float32] public int Value { get; set; }
            }
            """);

        Diagnostic diagnostic = AssertSingle(run, "NTC009", DiagnosticSeverity.Error, "int Value");
        Assert.Contains("[Float32] does not suit the type int", MessageOf(diagnostic));
    }

    [Fact]
    public void CountOnANonIntegerRaisesNtc009()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [Count, Float32] public float Length { get; set; }
            }
            """);

        Diagnostic diagnostic = AssertSingle(run, "NTC009", DiagnosticSeverity.Error, "float Length");
        Assert.Contains("[Count] member must be a settable integer", MessageOf(diagnostic));
    }

    [Fact]
    public void UnionOverAnInterfaceThatIsNotAWireStructRaisesNtc009()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public interface IArm
            {
            }

            [WireStruct]
            public partial class Holder
            {
                [UInt(8)] public byte Selector { get; set; }

                [Union(nameof(Selector))] public IArm Body { get; set; } = null!;
            }
            """);

        Diagnostic diagnostic = AssertSingle(run, "NTC009", DiagnosticSeverity.Error, "IArm Body");
        Assert.Contains("IWireStruct", MessageOf(diagnostic));
        Assert.DoesNotContain("Tests.Holder.g.cs", run.GeneratedSources);
    }

    [Fact]
    public void UnionWhoseInterfaceHasNoArmRaisesNtc010()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public interface IArm : IWireStruct
            {
            }

            [WireStruct]
            public partial class Holder
            {
                [UInt(8)] public byte Selector { get; set; }

                [Union(nameof(Selector))] public IArm Body { get; set; } = null!;
            }
            """);

        AssertSingle(run, "NTC010", DiagnosticSeverity.Error, "IArm Body");
        Assert.DoesNotContain("Tests.Holder.g.cs", run.GeneratedSources);
    }

    [Fact]
    public void AlignedBlobInAWireStructRaisesNtc011()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [Blob(16, Align = true)] public byte[] Payload { get; set; } = [];
            }
            """);

        AssertSingle(run, "NTC011", DiagnosticSeverity.Error, "byte[] Payload");
    }

    [Fact]
    public void AlignedBlobInAWireMessageRaisesNothing()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;
            using NexusTomorrow.Network.Message;

            namespace Tests;

            [WireMessage(GameMessageOpcode.ServerHello, Direction.ServerToClient)]
            public sealed partial class Sample
            {
                [Blob(16, Align = true)] public byte[] Payload { get; set; } = [];
            }
            """);

        AssertClean(run);
    }

    [Fact]
    public void TypeWithAnErrorGetsNoSourceWhileAValidTypeDoes()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Good
            {
                [UInt(8)] public byte Value { get; set; }
            }

            [WireStruct]
            public partial class Bad
            {
                public byte Missing { get; set; }
            }
            """);

        AssertSingle(run, "NTC001", DiagnosticSeverity.Error, "byte Missing");
        Assert.Contains("Tests.Good.g.cs", run.GeneratedSources);
        Assert.DoesNotContain("Tests.Bad.g.cs", run.GeneratedSources);
        Assert.Empty(run.CompilationErrors);
    }

    [Fact]
    public void NotOnWireMembersAreLeftOutOfTheLayout()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [UInt(8)] public byte Value { get; set; }

                [NotOnWire] public int Cached { get; set; }

                [NotOnWire] public int Doubled => Value * 2;
            }
            """);

        AssertClean(run);
        string generated = Assert.Contains("Tests.Sample.g.cs", run.GeneratedSources);
        Assert.DoesNotContain("Cached", generated);
        Assert.DoesNotContain("Doubled", generated);
    }

    // [WireStruct] targets AttributeTargets.Class, which admits a record class; skipping one silently
    // would move the failure into code that uses it.
    [Fact]
    public void RecordRaisesNtc012AndGetsNoSource()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial record Sample
            {
                [UInt(8)] public byte Value { get; set; }
            }
            """);

        AssertSingle(run, "NTC012", DiagnosticSeverity.Error, "record Sample");
        Assert.DoesNotContain("Tests.Sample.g.cs", run.GeneratedSources);
    }

    [Fact]
    public void GenericClassRaisesNtc012AndGetsNoSource()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample<T>
            {
                [UInt(8)] public byte Value { get; set; }
            }
            """);

        AssertSingle(run, "NTC012", DiagnosticSeverity.Error, "class Sample<T>");
        Assert.DoesNotContain(run.GeneratedSources.Keys, hint => hint.StartsWith("Tests.Sample", StringComparison.Ordinal));
    }

    [Fact]
    public void CountWiderThan32BitsRaisesNtc002()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [Count, UInt(40)] public ulong Length { get; set; }

                [Array(CountFrom = nameof(Length)), UInt(8)] public byte[] Values { get; set; } = [];
            }
            """);

        AssertHas(run, "NTC002", DiagnosticSeverity.Error, "ulong Length");
        Assert.DoesNotContain("Tests.Sample.g.cs", run.GeneratedSources);
    }

    [Fact]
    public void UnionCaseTheSelectorCannotHoldRaisesNtc003AndTheHolderGetsNoSource()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public interface IArm : IWireStruct
            {
            }

            [WireStruct]
            [UnionCase(0)]
            public sealed partial class Zero : IArm
            {
            }

            [WireStruct]
            [UnionCase(200)]
            public sealed partial class Wide : IArm
            {
            }

            [WireStruct]
            public partial class Holder
            {
                [SInt(8)] public sbyte Selector { get; set; }

                [Union(nameof(Selector))] public IArm Arm { get; set; } = new Zero();
            }
            """);

        Diagnostic diagnostic = AssertSingle(run, "NTC003", DiagnosticSeverity.Error, "sbyte Selector");
        Assert.Contains("the [UnionCase] of Wide = 200", MessageOf(diagnostic));
        Assert.DoesNotContain("Tests.Holder.g.cs", run.GeneratedSources);
    }

    [Fact]
    public void GetOnlyScalarRaisesNtc009()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [UInt(8)] public byte Value { get; }
            }
            """);

        Diagnostic diagnostic = AssertSingle(run, "NTC009", DiagnosticSeverity.Error, "byte Value");
        Assert.Contains("needs a setter", MessageOf(diagnostic));
        Assert.DoesNotContain("Tests.Sample.g.cs", run.GeneratedSources);
    }

    // GeneratorHarness.Run throws when the generator does, so reaching the assertions is the test.
    [Fact]
    public void AttributeMissingItsArgumentLeavesTheCompilerError()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public partial class Sample
            {
                [UInt()] public byte Value { get; set; }
            }
            """);

        Assert.Contains(run.CompilationErrors, error => error.Id == "CS7036");
        Assert.DoesNotContain("Tests.Sample.g.cs", run.GeneratedSources);
    }

    private static void AssertHas(GeneratorRun run, string id, DiagnosticSeverity severity, string pointsAt)
    {
        int line = GeneratorHarness.LineOf(run.Source, pointsAt);
        Assert.Contains(run.Diagnostics, diagnostic => diagnostic.Id == id && diagnostic.Severity == severity && GeneratorHarness.LineOf(diagnostic) == line);
    }

    private static Diagnostic AssertSingle(GeneratorRun run, string id, DiagnosticSeverity severity, string pointsAt)
    {
        Diagnostic diagnostic = Assert.Single(run.Diagnostics);
        Assert.Equal(id, diagnostic.Id);
        Assert.Equal(severity, diagnostic.Severity);
        Assert.Equal(GeneratorHarness.LineOf(run.Source, pointsAt), GeneratorHarness.LineOf(diagnostic));
        return diagnostic;
    }

    private static void AssertClean(GeneratorRun run)
    {
        Assert.Empty(run.Diagnostics);
        Assert.Empty(run.CompilationErrors);
    }

    private static string MessageOf(Diagnostic diagnostic)
    {
        return diagnostic.GetMessage(CultureInfo.InvariantCulture);
    }
}
