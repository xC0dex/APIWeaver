namespace APIWeaver.Schema.Exceptions;

/// <summary>
/// Exception that is thrown when a contract is not found for a given type.
/// </summary>
public sealed class ContractResolverNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContractResolverNotFoundException" /> class.
    /// </summary>
    public ContractResolverNotFoundException(MemberInfo type) : base($"No contract found for type `{type.Name}`")
    {
    }
}