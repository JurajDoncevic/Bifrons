﻿using Bifrons.Lenses.Tests;

namespace Bifrons.Lenses.Strings.Tests;

public class CopyLensTests : AsymmetricLensTestingFramework<string, string>
{
    protected override string _source => "Hello, World!";
    protected override string _view => "World";
    protected override string _updatedView => "Universe";

    protected override BaseAsymmetricLens<string, string> _lens => new CopyLens(@"World");

}