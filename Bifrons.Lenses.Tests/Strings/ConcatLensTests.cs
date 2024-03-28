using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Strings.Tests;

public class ConcatLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "12345Jane;Doe";

    protected override string _right => "Jane Doe";

    private readonly string _nameRegex = @"[a-zA-Z]+";
    private readonly string _numberRegex = @"12345";

    protected override ISymmetricLens<string, string> _lens
        => DeleteLens.Cons(_numberRegex) & IdentityLens.Cons(_nameRegex) & DeleteLens.Cons(";") & InsertLens.Cons(" ") & IdentityLens.Cons(_nameRegex);

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => ("12345Jane;Doe", "Jane Doe", "Janine Doe", "12345Janine;Doe");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => ("Jane Doe", "12345Jane;Doe", "12345Janine;Doe", "Janine Doe");
}
