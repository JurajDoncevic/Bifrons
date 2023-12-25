using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public sealed class CopyLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "Hello, World!";

    protected override string _right => "World";

    protected override string _updatedLeft => "Hello, Universe!";

    protected override string _updatedRight => "Universe";

    // moving right: ignore the part of the string until a comma is found
    // moving left: take the entire string
    protected override BaseSymmetricLens<string, string> _lens => CopyLens.Cons(@"(?!.*, )\w+", @"\w+");
}
