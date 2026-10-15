using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NexusTomorrow.ModelConverter;

/// <summary>
/// Reads a legacy <c>Read(GamePacketReader)</c> body, statement by statement, into raw operations. A statement the
/// patterns below do not cover stops the reading with its text as the reason.
/// </summary>
internal sealed class ReadBodyReader
{
    private readonly SemanticModel _model;
    private readonly INamedTypeSymbol _type;
    private readonly IParameterSymbol _reader;
    private readonly List<RawOp> _ops = [];
    private readonly HashSet<string> _counts = [];
    private ISymbol? _created;
    private string? _failure;

    private ReadBodyReader(SemanticModel model, INamedTypeSymbol type, IParameterSymbol reader)
    {
        _model = model;
        _type = type;
        _reader = reader;
    }

    public static (List<RawOp> Ops, string? Failure) Read(SemanticModel model, INamedTypeSymbol type, MethodDeclarationSyntax method)
    {
        if (model.GetDeclaredSymbol(method) is not IMethodSymbol { Parameters: [var reader] })
            return ([], "Read does not take one reader");

        var body = new ReadBodyReader(model, type, reader);
        foreach (StatementSyntax statement in BodyStatements.Of(method))
        {
            body.ReadStatement(statement);
            if (body._failure is not null)
                return ([], body._failure);
        }

        if (body._created is not null)
            return ([], $"Read: {body._created.Name} is created but never read");

        return (body._ops, null);
    }

    private void ReadStatement(StatementSyntax statement)
    {
        if (_created is not null && !IsReadOf(statement, _created))
        {
            Fail(statement);
            return;
        }

        switch (statement)
        {
            case EmptyStatementSyntax:
                break;
            case LocalDeclarationStatementSyntax { Declaration.Variables: [{ Initializer: null }] }:
                break;
            case ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax { RawKind: (int)Microsoft.CodeAnalysis.CSharp.SyntaxKind.SimpleAssignmentExpression, Left: IdentifierNameSyntax target } assignment }
                when _model.GetSymbolInfo(target).Symbol is ILocalSymbol local && ReaderCall(assignment.Right) is { } countCall && CountBits(countCall) is int countBits:
                _counts.Add(local.Name);
                _ops.Add(new CountOp(null, local.Name, countBits));
                break;
            case ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax { RawKind: (int)Microsoft.CodeAnalysis.CSharp.SyntaxKind.SimpleAssignmentExpression } assignment }:
                ReadAssignment(assignment, statement);
                break;
            case ExpressionStatementSyntax { Expression: InvocationExpressionSyntax invocation }:
                ReadInvocation(invocation, statement);
                break;
            case LocalDeclarationStatementSyntax { Declaration.Variables: [{ Initializer.Value: var value } variable] } when ReaderCall(value) is { } call && CountBits(call) is int bits:
                _counts.Add(variable.Identifier.ValueText);
                _ops.Add(new CountOp(null, variable.Identifier.ValueText, bits));
                break;
            case ForStatementSyntax loop:
                ReadFor(loop);
                break;
            default:
                Fail(statement);
                break;
        }
    }

    private void ReadAssignment(AssignmentExpressionSyntax assignment, StatementSyntax statement)
    {
        ISymbol? member = MemberOf(assignment.Left);
        if (member is null)
        {
            Fail(statement);
            return;
        }

        ExpressionSyntax value = WriteBodyReader.Strip(assignment.Right);
        if (VectorOfReads(value) is { } vector)
        {
            _ops.Add(new ScalarOp(member, vector.Encoding, vector.Bits));
            return;
        }

        if (value is BaseObjectCreationExpressionSyntax creation)
        {
            if (creation.Initializer is not null || creation.ArgumentList?.Arguments.Count > 0)
                Fail(statement);
            else
                _created = member;
            return;
        }

        if (value is ArrayCreationExpressionSyntax)
            return;

        if (ReaderCall(value) is not { } call || _model.GetSymbolInfo(call).Symbol is not IMethodSymbol method)
        {
            Fail(statement);
            return;
        }

        if (method.Name == "ReadBytes")
        {
            ExpressionSyntax length = call.ArgumentList.Arguments[0].Expression;
            int? constant = ConstantInt(length);
            string? local = WriteBodyReader.Strip(length) is IdentifierNameSyntax name && _counts.Contains(name.Identifier.ValueText) ? name.Identifier.ValueText : null;
            if (constant is null && local is null)
            {
                Fail(statement);
                return;
            }

            _ops.Add(new BytesOp(member, constant, local));
            return;
        }

        RawOp? op = OpOfReaderMethod(member, method, call);
        if (op is null)
            Fail(statement);
        else
            _ops.Add(op);
    }

    private void ReadInvocation(InvocationExpressionSyntax invocation, StatementSyntax statement)
    {
        if (_model.GetSymbolInfo(invocation).Symbol is not IMethodSymbol method)
        {
            Fail(statement);
            return;
        }

        ExpressionSyntax? receiver = (invocation.Expression as MemberAccessExpressionSyntax)?.Expression;
        if (IsReader(receiver))
        {
            int bits = ReadBits(method, invocation);
            if (bits > 0 && method.Name != "ResetBits")
                _ops.Add(new PadOp(bits));
            else
                Fail(statement);
            return;
        }

        if (receiver is BaseExpressionSyntax && method.Name == "Read")
        {
            _ops.Add(new BaseOp());
            return;
        }

        if (method.Name == "Read" && invocation.ArgumentList.Arguments is [var only] && IsReader(only.Expression) && MemberOf(receiver) is { } nested)
        {
            if (_created is not null && !SymbolEqualityComparer.Default.Equals(_created, nested))
            {
                Fail(statement);
                return;
            }

            _created = null;
            _ops.Add(new StructOp(nested));
            return;
        }

        Fail(statement);
    }

    private void ReadFor(ForStatementSyntax loop)
    {
        if (loop.Condition is not BinaryExpressionSyntax { RawKind: (int)Microsoft.CodeAnalysis.CSharp.SyntaxKind.LessThanExpression } condition)
        {
            Fail(loop);
            return;
        }

        ExpressionSyntax bound = WriteBodyReader.Strip(condition.Right);
        string? countLocal = bound is IdentifierNameSyntax name && _counts.Contains(name.Identifier.ValueText) ? name.Identifier.ValueText : null;
        int? fixedLength = countLocal is null ? ConstantInt(bound) ?? LengthOfFixedArray(bound) : null;
        if (countLocal is null && fixedLength is null)
        {
            Fail(loop);
            return;
        }

        (ISymbol? list, ElementOp? element) = ElementOfLoopBody(loop.Statement);
        if (list is null || element is null)
        {
            Fail(loop);
            return;
        }

        _ops.Add(new LoopOp(list, element, countLocal, fixedLength));
    }

    /// <summary>
    /// The list a loop body fills and the element it reads: <c>L.Add(reader.ReadX())</c>, <c>L[i] = reader.ReadX()</c>, or
    /// <c>var e = new T(); e.Read(reader); L.Add(e);</c> and its <c>L[i] = new T(); L[i].Read(reader);</c> form.
    /// </summary>
    private (ISymbol? List, ElementOp? Element) ElementOfLoopBody(StatementSyntax body)
    {
        List<StatementSyntax> statements = body is BlockSyntax block ? [.. BodyStatements.Flatten(block.Statements)] : [body];
        if (statements is [ExpressionStatementSyntax { Expression: var single }])
            return ElementOfSingle(single);

        if (statements is [LocalDeclarationStatementSyntax { Declaration.Variables: [{ Initializer.Value: var created } local] }, ExpressionStatementSyntax { Expression: InvocationExpressionSyntax read }, ExpressionStatementSyntax { Expression: InvocationExpressionSyntax add }]
            && WriteBodyReader.Strip(created) is ObjectCreationExpressionSyntax or ImplicitObjectCreationExpressionSyntax
            && IsReadCallOn(read, local.Identifier.ValueText)
            && add is { Expression: MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Add" } addAccess, ArgumentList.Arguments: [var added] }
            && added.Expression is IdentifierNameSyntax addedName && addedName.Identifier.ValueText == local.Identifier.ValueText)
        {
            return (MemberOf(addAccess.Expression), new ElementOp(null, 0, null));
        }

        if (statements is [ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax { Left: ElementAccessExpressionSyntax target, Right: var newElement } }, ExpressionStatementSyntax { Expression: InvocationExpressionSyntax readElement }]
            && WriteBodyReader.Strip(newElement) is ObjectCreationExpressionSyntax or ImplicitObjectCreationExpressionSyntax
            && readElement.Expression is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Read", Expression: ElementAccessExpressionSyntax readTarget }
            && readTarget.ToString() == target.ToString())
        {
            return (MemberOf(target.Expression), new ElementOp(null, 0, null));
        }

        return (null, null);
    }

    private (ISymbol? List, ElementOp? Element) ElementOfSingle(ExpressionSyntax expression)
    {
        (ISymbol? list, ExpressionSyntax? value) = expression switch
        {
            InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Add" } access, ArgumentList.Arguments: [var argument] } => (MemberOf(access.Expression), argument.Expression),
            AssignmentExpressionSyntax { Left: ElementAccessExpressionSyntax target, Right: var right } => (MemberOf(target.Expression), right),
            _ => (null, null),
        };
        if (list is null || value is null || ReaderCall(WriteBodyReader.Strip(value)) is not { } call || _model.GetSymbolInfo(call).Symbol is not IMethodSymbol method)
            return (null, null);

        return method.Name switch
        {
            "ReadWideString" => (list, new ElementOp(null, 0, "WideString")),
            "ReadString" => (list, new ElementOp(null, 0, "NarrowString")),
            "ReadWideStringFixed" => (list, new ElementOp(null, 0, "TerminatedWideString")),
            "ReadPackedFloat" or "ReadPackedVector3" => (list, new ElementOp("PackedFloat", 16, null)),
            "ReadVector3" => (list, new ElementOp("Float32", 32, null)),
            "ReadSingle" => (list, new ElementOp("Float32", ReadBits(method, call), null)),
            "ReadDouble" => (list, new ElementOp("Float64", ReadBits(method, call), null)),
            _ => ReadBits(method, call) is > 0 and var bits ? (list, new ElementOp("UInt", bits, null)) : (null, null),
        };
    }

    /// <summary>
    /// A Vector2, Vector3 or Quaternion built from one read per component, every one <c>ReadSingle()</c> or every one <c>ReadPackedFloat()</c>, which is that value on the wire.
    /// </summary>
    private (string Encoding, int Bits)? VectorOfReads(ExpressionSyntax value)
    {
        if (value is not ObjectCreationExpressionSyntax { ArgumentList.Arguments: var arguments } creation
            || _model.GetTypeInfo(creation).Type is not { } type
            || WriteBodyReader.ComponentsOf(type) is 0
            || WriteBodyReader.ComponentsOf(type) != arguments.Count)
        {
            return null;
        }

        HashSet<string> methods = [.. arguments.Select(argument => ReaderCall(argument.Expression) is { ArgumentList.Arguments.Count: 0 } call && _model.GetSymbolInfo(call).Symbol is IMethodSymbol method ? method.Name : "")];
        return methods.Count != 1 ? null : methods.Single() switch
        {
            "ReadSingle" => ("Float32", 32),
            "ReadPackedFloat" => ("PackedFloat", 16),
            _ => null,
        };
    }

    private RawOp? OpOfReaderMethod(ISymbol member, IMethodSymbol method, InvocationExpressionSyntax call)
    {
        return method.Name switch
        {
            "ReadWideString" => new StringOp(member, "WideString"),
            "ReadString" => new StringOp(member, "NarrowString"),
            "ReadWideStringFixed" => new StringOp(member, "TerminatedWideString"),
            "ReadPackedFloat" or "ReadPackedVector3" => new ScalarOp(member, "PackedFloat", 16),
            "ReadVector3" => new ScalarOp(member, "Float32", 32),
            "ReadSingle" => new ScalarOp(member, "Float32", ReadBits(method, call)),
            "ReadDouble" => new ScalarOp(member, "Float64", ReadBits(method, call)),
            _ => ReadBits(method, call) is > 0 and var bits ? new ScalarOp(member, "UInt", bits) : null,
        };
    }

    /// <summary>
    /// The width an integer read takes: 1 for <c>ReadBit</c>, else its explicit width argument or its parameter's default.
    /// </summary>
    private int ReadBits(IMethodSymbol method, InvocationExpressionSyntax call)
    {
        if (method.Name == "ReadBit")
            return 1;
        if (method.Name is not ("ReadByte" or "ReadUShort" or "ReadShort" or "ReadUInt" or "ReadInt" or "ReadULong" or "ReadLong" or "ReadEnum" or "ReadSingle" or "ReadDouble") || method.Parameters.Length != 1)
            return 0;

        SeparatedSyntaxList<ArgumentSyntax> arguments = call.ArgumentList.Arguments;
        if (arguments.Count == 1)
            return ConstantInt(arguments[0].Expression) ?? 0;

        return method.Parameters[0].HasExplicitDefaultValue ? Convert.ToInt32(method.Parameters[0].ExplicitDefaultValue, CultureInfo.InvariantCulture) : 0;
    }

    private int? CountBits(InvocationExpressionSyntax call)
    {
        return _model.GetSymbolInfo(call).Symbol is IMethodSymbol { Name: "ReadByte" or "ReadUShort" or "ReadUInt" or "ReadInt" or "ReadShort" } method && ReadBits(method, call) is > 0 and var bits ? bits : null;
    }

    private InvocationExpressionSyntax? ReaderCall(ExpressionSyntax expression)
    {
        return WriteBodyReader.Strip(expression) is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax access } call && IsReader(access.Expression) ? call : null;
    }

    private bool IsReadOf(StatementSyntax statement, ISymbol member)
    {
        return statement is ExpressionStatementSyntax { Expression: InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Read" } access } }
            && SymbolEqualityComparer.Default.Equals(MemberOf(access.Expression), member);
    }

    private bool IsReadCallOn(InvocationExpressionSyntax call, string local)
    {
        return call is { Expression: MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Read", Expression: IdentifierNameSyntax target }, ArgumentList.Arguments: [var argument] }
            && target.Identifier.ValueText == local
            && IsReader(argument.Expression);
    }

    private bool IsReader(ExpressionSyntax? expression)
    {
        return expression is not null && SymbolEqualityComparer.Default.Equals(_model.GetSymbolInfo(expression).Symbol, _reader);
    }

    private ISymbol? MemberOf(ExpressionSyntax? expression)
    {
        if (expression is null)
            return null;

        ExpressionSyntax stripped = WriteBodyReader.Strip(expression);
        if (stripped is not (IdentifierNameSyntax or MemberAccessExpressionSyntax { Expression: ThisExpressionSyntax }))
            return null;

        ISymbol? symbol = _model.GetSymbolInfo(stripped).Symbol;
        if (symbol is not ((IPropertySymbol or IFieldSymbol) and { IsStatic: false }))
            return null;

        for (INamedTypeSymbol? type = _type; type is not null; type = type.BaseType)
        {
            if (SymbolEqualityComparer.Default.Equals(symbol.ContainingType, type))
                return symbol;
        }

        return null;
    }

    private int? LengthOfFixedArray(ExpressionSyntax bound)
    {
        return bound is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Length" } access && MemberOf(access.Expression) is { } array
            ? FixedLengths.Of(array, _model.Compilation)
            : null;
    }

    private int? ConstantInt(ExpressionSyntax expression)
    {
        Optional<object?> constant = _model.GetConstantValue(expression);
        return constant.HasValue && constant.Value is not null and not bool ? Convert.ToInt32(constant.Value, CultureInfo.InvariantCulture) : null;
    }

    private void Fail(SyntaxNode statement)
    {
        _failure ??= "Read: " + statement.ToString().ReplaceLineEndings(" ");
    }
}
