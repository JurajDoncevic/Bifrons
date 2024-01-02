using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class ConcatLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "12345Jane;Doe";

    protected override string _right => "Jane Doe";

    private readonly string _nameRegex = @"[a-zA-Z]+";
    private readonly string _numberRegex = @"12345";

    protected override BaseSymmetricLens<string, string> _lens
        => DeleteLens.Cons(_numberRegex) & IdentityLens.Cons(_nameRegex) & DeleteLens.Cons(";") & InsertLens.Cons(" ") & IdentityLens.Cons(_nameRegex);
}
