using System;
using Bifrons.Lenses.Integers;

namespace Bifrons.Lenses.Tests.Integers;

public class Experiments
{
    [Fact]
    public void Paper_IntegerExample()
    {
        var l_id = IdentityLens.Cons();
        var l_add1 = AddLens.Cons(1);
        var l_add5 = AddLens.Cons(5);
        var l_sub3 = SubLens.Cons(3);

        var l_add6 = l_id >> l_add1 >> l_add5 >> l_sub3;

        var left = 10;
        var expectedRight = 13;

        var right = l_add6.CreateRight(left);

        Assert.True(right);
        Assert.Equal(expectedRight, right.Data);
    }
}
