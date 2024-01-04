using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class IterateLensTests : SymmetricLensTestingFramework<string, IEnumerable<string>>
{
    protected override string _left => "John\nPaul\nAlice\nGeorge\nDicky\nStuart\nPete";

    protected override IEnumerable<string> _right => new[] { "John", "Paul", "Alice", "George", "Dicky", "Stuart", "Pete" };

    protected override BaseSymmetricLens<string, IEnumerable<string>> _lens =>
        EnumerateLens.Cons(
            "\n",
            IdentityLens.Cons(@"\w+")
        );
}
