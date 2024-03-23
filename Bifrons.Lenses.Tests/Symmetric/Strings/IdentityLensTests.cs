using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class IdentityLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "Hello, World!";

    protected override string _right => "Hello, World!";

    private readonly string _greetingRegex = @"Hello, [a-zA-Z]+!";
    protected override ISimpleSymmetricLens<string, string> _lens => IdentityLens.Cons(_greetingRegex);

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithRightSideUpdateData
        => ("Hello, World!", "Hello, World!", "Hello, Universe!", "Hello, Universe!");

    protected override (string originalSource, string expectedOriginalTarget, string updatedTarget, string expectedUpdatedSource) _roundTripWithLeftSideUpdateData
        => ("Hello, World!", "Hello, World!", "Hello, Universe!", "Hello, Universe!");
}
