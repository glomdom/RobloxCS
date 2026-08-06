using System.Collections.Frozen;
using System.Text;
using Microsoft.CodeAnalysis;
using RobloxCS.AST.Statements;

namespace RobloxCS.Transpiler;

public sealed class LoweringContext {
    public List<Statement> Prologue { get; } = [];

    private static readonly FrozenSet<string> Reserved = new[] {
        "and", "break", "do", "else", "elseif", "end", "false", "for", "function",
        "if", "in", "local", "nil", "not", "or", "repeat", "return", "then",
        "true", "until", "while",

        "continue", "export", "type",

        "_G", "assert", "error", "game", "getmetatable", "ipairs", "math", "next",
        "pairs", "pcall", "print", "rawequal", "rawget", "rawlen", "rawset",
        "require", "script", "select", "self", "setmetatable", "string", "table",
        "tonumber", "tostring", "typeof", "unpack", "workspace",
    }.ToFrozenSet(StringComparer.Ordinal);

    private readonly Stack<List<Statement>> _buffers = new();
    private readonly Dictionary<ISymbol, string> _names = new(SymbolEqualityComparer.Default);
    private readonly HashSet<string> _taken = new(StringComparer.Ordinal);

    private int _tempCounter;

    public void Emit(Statement stmt) {
        if (_buffers.Count == 0) {
            throw new InvalidOperationException("Emit called outside of a Capture scope.");
        }

        _buffers.Peek().Add(stmt);
    }

    public void BeginFunction() {
        _names.Clear();
        _taken.Clear();

        _tempCounter = 0;
    }

    public string NameFor(ISymbol symbol) {
        if (_names.TryGetValue(symbol, out var existing)) {
            return existing;
        }

        var name = Claim(Sanitize(symbol.Name));
        _names[symbol] = name;

        return name;
    }

    public string FreshTemp(string hint) {
        var candidate = $"__{Sanitize(hint)}{++_tempCounter}"; // this sucks

        return Claim(candidate);
    }

    private string Claim(string candidate) {
        var name = Reserved.Contains(candidate) ? candidate + "_" : candidate;
        if (_taken.Add(name)) {
            return name;
        }

        for (var index = 2;; index++) {
            var suffixed = $"{name}_{index}";
            if (_taken.Add(suffixed)) {
                return suffixed;
            }
        }
    }

    private static string Sanitize(string raw) {
        if (string.IsNullOrEmpty(raw)) {
            return "_";
        }

        var builder = new StringBuilder(raw.Length + 1);

        foreach (var character in raw) {
            builder.Append(char.IsAsciiLetterOrDigit(character) || character == '_' ? character : '_');
        }

        if (char.IsAsciiDigit(builder[0])) {
            builder.Insert(0, '_');
        }

        return builder.ToString();
    }

    public List<Statement> Capture(Action body) {
        _buffers.Push([]);

        try {
            body();

            return _buffers.Pop();
        } catch {
            _buffers.Pop();

            throw;
        }
    }
}