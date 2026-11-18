namespace Ataskado.Core.Domain;

public sealed record Error(
    string Code,
    string Description,
    ErrorType Type);
