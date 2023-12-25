﻿using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class IdentityLensTests : SymmetricLensTestingFramework<string, string>
{
    protected override string _left => "Hello, World!";

    protected override string _right => "Hello, World!";

    protected override string _updatedLeft => "Hello, Universe!";

    protected override string _updatedRight => "Hello, Universe!";

    protected override BaseSymmetricLens<string, string> _lens => IdentityLens.Cons();
}
