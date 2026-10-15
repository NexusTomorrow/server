using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NexusTomorrow.ModelConverter;

/// <summary>
/// Reads a legacy <c>Write(GamePacketWriter)</c> body, statement by statement, into raw operations. A statement the
/// patterns below do not cover stops the reading with its text as the reason.
/// </summary>
internal sealed class WriteBodyReader
{
    private readonly SemanticModel _model;
    private readonly INamedTypeSymbol _type;
    private readonly IParameterSymbol _writer;
    private readonly List<RawOp> _ops = [];
    private (ISymbol Vector, char Next, string Encoding)? _vector;
    private string? _failure;

    private WriteBodyReader(SemanticModel model, INamedTypeSymbol type, IParameterSymbol writer)
    {
        _model = model;
        _type = type;
        _writer = writer;
    }

    public static (List<RawOp> Ops, string? Failure) Read(SemanticModel model, INamedTypeSymbol type, MethodDeclarationSyntax method)
    {
        if (model.GetDeclaredSymbol(method) is not IMethodSymbol { Parameters: [var writer] })
            return ([], "Write does not take one writer");

        var reader = new WriteBodyReader(model, type, writer);
        foreach (StatementSyntax statement in BodyStatements.Of(method))
        {
            reader.ReadStatement(statement);
            if (reader._failure is not null)
                return ([], reader._failure);
        }

        if (reader._vector is { } open)
            return ([], $"Write: {open.Vector.Name} is written in part");

        return (reader._ops, null);
    }

    /// <summary>
    /// Take the components of one Vector2 or Vector3 written in turn, in one encoding, as the one vector they are on the wire.
    /// </summary>
    private void AddVectorComponent(ISymbol vector, char axis, string encoding)
    {
        bool plane = MemberBuilder.TypeOf(vector).ToDisplayString() == "System.Numerics.Vector2";
        int bits = encoding == "Float32" ? 32 : 16;
        switch (axis)
        {
            case 'X' when _vector is null:
                _vector = (vector, 'Y', encoding);
                break;
            case 'Y' when plane:
            case 'Z':
                _vector = null;
                _ops.Add(new ScalarOp(vector, encoding, bits));
                break;
            case 'Y':
                _vector = (vector, 'Z', encoding);
                break;
            default:
                _failure ??= $"Write: {vector.Name}.{axis} is written out of order";
                break;
        }
    }

    private void ReadStatement(StatementSyntax statement)
    {
        if (_vector is not null && !IsVectorComponent(statement))
        {
            Fail(statement);
            return;
        }

        switch (statement)
        {
            case EmptyStatementSyntax:
                break;
            case ExpressionStatementSyntax { Expression: InvocationExpressionSyntax invocation }:
                ReadInvocation(invocation, statement);
                break;
            case ForEachStatementSyntax loop:
                ReadForEach(loop);
                break;
            case ForStatementSyntax loop:
                ReadFor(loop);
                break;
            default:
                Fail(statement);
                break;
        }
    }

    /// <summary>
    /// A <c>for</c> loop writing <c>X[i]</c> once per index: up to a constant, <c>X.Length</c> or <c>X.Count</c>.
    /// </summary>
    private void ReadFor(ForStatementSyntax loop)
    {
        if (loop is not { Declaration.Variables: [var index], Condition: BinaryExpressionSyntax { RawKind: (int)SyntaxKind.LessThanExpression } condition }
            || BodyStatements.Single(loop.Statement) is not ExpressionStatementSyntax { Expression: InvocationExpressionSyntax body })
        {
            Fail(loop);
            return;
        }

        ExpressionSyntax bound = Strip(condition.Right);
        ISymbol? boundList = bound is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Length" or "Count" } access ? MemberOf(access.Expression) : null;
        int? constant = ConstantInt(bound);
        (ISymbol? list, ElementOp? element) = ElementOfIndexedCall(body, index.Identifier.ValueText);
        if (list is null || element is null || (boundList is not null && !SymbolEqualityComparer.Default.Equals(boundList, list)) || (boundList is null && constant is null))
        {
            Fail(loop);
            return;
        }

        _ops.Add(new LoopOp(list, element, null, constant ?? FixedUnlessCounted(list)));
    }

    /// <summary>
    /// The list and element of one call on <c>X[i]</c>: <c>X[i].Write(writer)</c> or a writer call on <c>X[i]</c>.
    /// </summary>
    private (ISymbol? List, ElementOp? Element) ElementOfIndexedCall(InvocationExpressionSyntax call, string index)
    {
        if (_model.GetSymbolInfo(call).Symbol is not IMethodSymbol method || call.Expression is not MemberAccessExpressionSyntax access)
            return (null, null);

        SeparatedSyntaxList<ArgumentSyntax> arguments = call.ArgumentList.Arguments;
        if (method.Name == "Write" && IndexedMember(access.Expression, index) is { } nested && arguments is [var writer] && IsWriter(writer.Expression))
            return (nested, new ElementOp(null, 0, null));

        if (IsWriter(access.Expression) && arguments.Count > 0 && IndexedMember(arguments[0].Expression, index) is { } list)
            return (list, ElementOfWriterMethod(method, arguments));

        return (null, null);
    }

    private ISymbol? IndexedMember(ExpressionSyntax expression, string index)
    {
        return Strip(expression) is ElementAccessExpressionSyntax { ArgumentList.Arguments: [var argument] } element
            && argument.Expression is IdentifierNameSyntax name && name.Identifier.ValueText == index
            ? MemberOf(element.Expression)
            : null;
    }

    /// <summary>
    /// Whether the statement writes the next component of the Vector3 whose X was written, as <c>WriteVector3</c> would.
    /// </summary>
    private bool IsVectorComponent(StatementSyntax statement)
    {
        string method = _vector?.Encoding == "PackedFloat" ? "WritePackedFloat" : "Write";
        return statement is ExpressionStatementSyntax { Expression: InvocationExpressionSyntax { ArgumentList.Arguments: [var argument] } invocation }
            && invocation.Expression is MemberAccessExpressionSyntax access
            && access.Name.Identifier.ValueText == method
            && IsWriter(access.Expression)
            && VectorComponent(argument.Expression) is { } component
            && SymbolEqualityComparer.Default.Equals(component.Vector, _vector?.Vector)
            && component.Axis == _vector?.Next;
    }

    private (ISymbol Vector, char Axis)? VectorComponent(ExpressionSyntax? expression)
    {
        return expression is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "X" or "Y" or "Z" } access
            && MemberOf(access.Expression) is { } vector
            && MemberBuilder.TypeOf(vector).ToDisplayString() is "System.Numerics.Vector3" or "System.Numerics.Vector2"
            ? (vector, access.Name.Identifier.ValueText[0])
            : null;
    }

    private void ReadInvocation(InvocationExpressionSyntax invocation, SyntaxNode statement)
    {
        if (_model.GetSymbolInfo(invocation).Symbol is not IMethodSymbol method)
        {
            Fail(statement);
            return;
        }

        ExpressionSyntax? receiver = (invocation.Expression as MemberAccessExpressionSyntax)?.Expression;
        if (IsWriter(receiver))
        {
            ReadWriterCall(invocation, method, statement);
            return;
        }

        if (receiver is BaseExpressionSyntax && method.Name == "Write")
        {
            _ops.Add(new BaseOp());
            return;
        }

        if (method.Name == "Write" && invocation.ArgumentList.Arguments is [var only] && IsWriter(only.Expression) && MemberOf(receiver) is { } nested)
        {
            _ops.Add(new StructOp(nested));
            return;
        }

        if (method.Name == "ForEach" && invocation.ArgumentList.Arguments is [var action] && MemberOf(receiver) is { } list)
        {
            ElementOp? element = ElementOfAction(action.Expression);
            if (element is null)
            {
                Fail(statement);
                return;
            }

            _ops.Add(new LoopOp(list, element, null, FixedUnlessCounted(list)));
            return;
        }

        Fail(statement);
    }

    private void ReadWriterCall(InvocationExpressionSyntax invocation, IMethodSymbol method, SyntaxNode statement)
    {
        SeparatedSyntaxList<ArgumentSyntax> arguments = invocation.ArgumentList.Arguments;
        ExpressionSyntax? value = arguments.Count > 0 ? arguments[0].Expression : null;
        switch (method.Name)
        {
            case "Write" when value is not null && method.Parameters[0].Type is IArrayTypeSymbol:
                {
                    int? elements = ConstantInt(arguments.Count > 1 ? arguments[1].Expression : null);
                    if (MemberOf(value) is not { } list)
                        break;

                    _ops.Add(new LoopOp(list, new ElementOp("UInt", 64, null), null, elements is > 0 ? elements : FixedUnlessCounted(list)));
                    return;
                }

            case "Write" when value is not null:
                {
                    (string encoding, int bits) = ScalarOfWrite(method, arguments);
                    if (bits <= 0)
                        break;

                    if (CountedList(value) is { } counted)
                    {
                        _ops.Add(new CountOp(counted, null, bits));
                        return;
                    }

                    if (IsZero(value))
                    {
                        _ops.Add(new PadOp(bits));
                        return;
                    }

                    if (encoding == "Float32" && bits == 32 && VectorComponent(value) is { } component)
                    {
                        AddVectorComponent(component.Vector, component.Axis, "Float32");
                        return;
                    }

                    if (MemberOf(value) is { } member)
                    {
                        _ops.Add(new ScalarOp(member, encoding, bits));
                        return;
                    }

                    break;
                }

            case "WriteStringWide" or "WriteString" or "WriteStringFixed" when MemberOf(value) is { } member:
                _ops.Add(new StringOp(member, StringAttribute(method.Name)));
                return;
            case "WritePackedFloat" when VectorComponent(value) is { } packed:
                AddVectorComponent(packed.Vector, packed.Axis, "PackedFloat");
                return;
            case "WritePackedFloat" or "WritePackedVector3" when MemberOf(value) is { } member:
                _ops.Add(new ScalarOp(member, "PackedFloat", 16));
                return;
            case "WriteVector3" when MemberOf(value) is { } member:
                _ops.Add(new ScalarOp(member, "Float32", 32));
                return;
            case "WriteBytes" when MemberOf(value) is { } member:
                {
                    int? length = ConstantInt(arguments.Count > 1 ? arguments[1].Expression : null);
                    _ops.Add(new BytesOp(member, length is > 0 ? length : FixedUnlessCounted(member), null));
                    return;
                }
        }

        Fail(statement);
    }

    private void ReadForEach(ForEachStatementSyntax loop)
    {
        if (MemberOf(loop.Expression) is not { } list || BodyStatements.Single(loop.Statement) is not ExpressionStatementSyntax { Expression: InvocationExpressionSyntax body })
        {
            Fail(loop);
            return;
        }

        ElementOp? element = ElementOfCall(body, loop.Identifier.ValueText);
        if (element is null)
        {
            Fail(loop);
            return;
        }

        _ops.Add(new LoopOp(list, element, null, FixedUnlessCounted(list)));
    }

    /// <summary>
    /// The element a <c>ForEach</c> argument writes: a lambda over one call, or a writer method group.
    /// </summary>
    private ElementOp? ElementOfAction(ExpressionSyntax action)
    {
        if (action is SimpleLambdaExpressionSyntax { ExpressionBody: InvocationExpressionSyntax call } lambda)
            return ElementOfCall(call, lambda.Parameter.Identifier.ValueText);

        if (action is ParenthesizedLambdaExpressionSyntax { ParameterList.Parameters: [var parameter], ExpressionBody: InvocationExpressionSyntax parenthesizedCall })
            return ElementOfCall(parenthesizedCall, parameter.Identifier.ValueText);

        if (action is MemberAccessExpressionSyntax group && IsWriter(group.Expression))
        {
            ISymbol? symbol = _model.GetSymbolInfo(group).Symbol ?? _model.GetSymbolInfo(group).CandidateSymbols.FirstOrDefault();
            if (symbol is IMethodSymbol method)
                return ElementOfWriterMethod(method, []);
        }

        return null;
    }

    /// <summary>
    /// The element one call writes, where <paramref name="element"/> names the loop variable: <c>e.Write(writer)</c> or a writer call on <c>e</c>.
    /// </summary>
    private ElementOp? ElementOfCall(InvocationExpressionSyntax call, string element)
    {
        if (_model.GetSymbolInfo(call).Symbol is not IMethodSymbol method || call.Expression is not MemberAccessExpressionSyntax access)
            return null;

        SeparatedSyntaxList<ArgumentSyntax> arguments = call.ArgumentList.Arguments;
        if (method.Name == "Write" && access.Expression is IdentifierNameSyntax target && target.Identifier.ValueText == element && arguments is [var writer] && IsWriter(writer.Expression))
            return new ElementOp(null, 0, null);

        if (IsWriter(access.Expression) && arguments.Count > 0 && Strip(arguments[0].Expression) is IdentifierNameSyntax value && value.Identifier.ValueText == element)
            return ElementOfWriterMethod(method, arguments);

        return null;
    }

    private ElementOp? ElementOfWriterMethod(IMethodSymbol method, SeparatedSyntaxList<ArgumentSyntax> arguments)
    {
        switch (method.Name)
        {
            case "Write":
                {
                    (string encoding, int bits) = ScalarOfWrite(method, arguments);
                    return bits > 0 ? new ElementOp(encoding, bits, null) : null;
                }

            case "WriteStringWide" or "WriteString" or "WriteStringFixed":
                return new ElementOp(null, 0, StringAttribute(method.Name));
            case "WritePackedFloat" or "WritePackedVector3":
                return new ElementOp("PackedFloat", 16, null);
            case "WriteVector3":
                return new ElementOp("Float32", 32, null);
            default:
                return null;
        }
    }

    /// <summary>
    /// The encoding and width a <c>Write</c> overload puts on the wire: its explicit width argument or its parameter's default.
    /// </summary>
    private (string Encoding, int Bits) ScalarOfWrite(IMethodSymbol method, SeparatedSyntaxList<ArgumentSyntax> arguments)
    {
        ITypeSymbol parameter = method.Parameters[0].Type;
        if (parameter.SpecialType == SpecialType.System_Boolean)
            return ("UInt", 1);

        int bits = method.Parameters.Length > 1
            ? ConstantInt(arguments.Count > 1 ? arguments[1].Expression : null) ?? (method.Parameters[1].HasExplicitDefaultValue ? Convert.ToInt32(method.Parameters[1].ExplicitDefaultValue, CultureInfo.InvariantCulture) : 0)
            : 0;
        string encoding = parameter.SpecialType switch
        {
            SpecialType.System_Single => "Float32",
            SpecialType.System_Double => "Float64",
            _ => "UInt",
        };
        return (encoding, bits);
    }

    private static string StringAttribute(string method)
    {
        return method switch
        {
            "WriteStringWide" => "WideString",
            "WriteString" => "NarrowString",
            _ => "TerminatedWideString",
        };
    }

    private bool IsWriter(ExpressionSyntax? expression)
    {
        return expression is not null && SymbolEqualityComparer.Default.Equals(_model.GetSymbolInfo(expression).Symbol, _writer);
    }

    /// <summary>
    /// The list or array whose <c>Count</c> or <c>Length</c> the expression is, through casts.
    /// </summary>
    private ISymbol? CountedList(ExpressionSyntax expression)
    {
        return Strip(expression) is MemberAccessExpressionSyntax { Name.Identifier.ValueText: "Count" or "Length" } access ? MemberOf(access.Expression) : null;
    }

    /// <summary>
    /// The instance property or field of the model the expression names, through parentheses, casts and <c>this.</c>.
    /// </summary>
    private ISymbol? MemberOf(ExpressionSyntax? expression)
    {
        if (expression is null)
            return null;

        ExpressionSyntax stripped = Strip(expression);
        if (stripped is not (IdentifierNameSyntax or MemberAccessExpressionSyntax { Expression: ThisExpressionSyntax }))
            return null;

        ISymbol? symbol = _model.GetSymbolInfo(stripped).Symbol;
        return symbol is (IPropertySymbol or IFieldSymbol) and { IsStatic: false } && IsOwnMember(symbol) ? symbol : null;
    }

    private bool IsOwnMember(ISymbol symbol)
    {
        for (INamedTypeSymbol? type = _type; type is not null; type = type.BaseType)
        {
            if (SymbolEqualityComparer.Default.Equals(symbol.ContainingType, type))
                return true;
        }

        return false;
    }

    /// <summary>
    /// The length of an array member initialised with <c>new T[N]</c>, unless a count of it was written just before, which makes it counted.
    /// </summary>
    private int? FixedUnlessCounted(ISymbol member)
    {
        if (_ops.LastOrDefault() is CountOp { List: { } counted } && SymbolEqualityComparer.Default.Equals(counted, member))
            return null;

        return FixedLengths.Of(member, _model.Compilation) is > 0 and var length ? length : null;
    }

    private bool IsZero(ExpressionSyntax expression)
    {
        Optional<object?> constant = _model.GetConstantValue(expression);
        return constant.HasValue && constant.Value switch
        {
            bool flag => !flag,
            null => false,
            var number => Convert.ToDecimal(number, CultureInfo.InvariantCulture) == 0,
        };
    }

    private int? ConstantInt(ExpressionSyntax? expression)
    {
        if (expression is null)
            return null;

        Optional<object?> constant = _model.GetConstantValue(expression);
        return constant.HasValue && constant.Value is not null and not bool ? Convert.ToInt32(constant.Value, CultureInfo.InvariantCulture) : null;
    }

    internal static ExpressionSyntax Strip(ExpressionSyntax expression)
    {
        while (true)
        {
            switch (expression)
            {
                case ParenthesizedExpressionSyntax parenthesized:
                    expression = parenthesized.Expression;
                    break;
                case CastExpressionSyntax cast:
                    expression = cast.Expression;
                    break;
                default:
                    return expression;
            }
        }
    }

    private void Fail(SyntaxNode statement)
    {
        _failure ??= "Write: " + statement.ToString().ReplaceLineEndings(" ");
    }
}

/// <summary>
/// The statements of a method or loop body, with plain nested blocks flattened.
/// </summary>
internal static class BodyStatements
{
    public static IEnumerable<StatementSyntax> Of(BaseMethodDeclarationSyntax method)
    {
        if (method.Body is { } body)
            return Flatten(body.Statements);
        if (method.ExpressionBody is { } arrow)
            return [SyntaxFactory.ExpressionStatement(arrow.Expression)];
        return [];
    }

    public static StatementSyntax? Single(StatementSyntax statement)
    {
        return statement is BlockSyntax block ? (Flatten(block.Statements).ToList() is [var only] ? only : null) : statement;
    }

    public static IEnumerable<StatementSyntax> Flatten(SyntaxList<StatementSyntax> statements)
    {
        foreach (StatementSyntax statement in statements)
        {
            if (statement is BlockSyntax block)
            {
                foreach (StatementSyntax inner in Flatten(block.Statements))
                    yield return inner;
            }
            else
            {
                yield return statement;
            }
        }
    }
}

/// <summary>
/// The fixed length of an array member: its initialiser <c>new T[N]</c> with a constant N.
/// </summary>
internal static class FixedLengths
{
    public static int? Of(ISymbol member, Compilation compilation)
    {
        if (MemberBuilder.TypeOf(member) is not IArrayTypeSymbol)
            return null;

        foreach (SyntaxReference reference in member.DeclaringSyntaxReferences)
        {
            EqualsValueClauseSyntax? initializer = reference.GetSyntax() switch
            {
                PropertyDeclarationSyntax property => property.Initializer,
                VariableDeclaratorSyntax variable => variable.Initializer,
                _ => null,
            };
            if (initializer?.Value is ArrayCreationExpressionSyntax { Type.RankSpecifiers: [{ Sizes: [var size] }] })
            {
                SemanticModel model = compilation.GetSemanticModel(initializer.SyntaxTree);
                Optional<object?> constant = model.GetConstantValue(size);
                if (constant.HasValue && constant.Value is int length)
                    return length;
            }
        }

        return null;
    }
}
