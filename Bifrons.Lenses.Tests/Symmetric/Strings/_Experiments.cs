namespace Bifrons.Lenses.Symmetric.Strings.Tests;

public class Experiments
{
    [Fact]
    public void CreateRight_WithConcatLens()
    {
        var lens = IdentityLens.Cons(@"\w+") |
            IdentityLens.Cons(@"\s+") |
            IdentityLens.Cons(@"(?!\w+\s+)\w+") |
            DeleteLens.Cons(@"(?!\w+\s+):\s+") |
            InsertLens.Cons(@",");

        var source = "Jane Doe: ";
        var expectedTarget = "Jane Doe,";

        var target = lens.CreateRight(source);

        Assert.Equal(expectedTarget, target);

    }

    [Fact]
    public void PutRight_OnConcatLens_WithNoOriginalSource()
    {
        var lens = IdentityLens.Cons(@"\w+") |
            IdentityLens.Cons(@"\s+") |
            IdentityLens.Cons(@"(?!\w+\s+)\w+") |
            DeleteLens.Cons(@"(?!\w+\s+):\s+") |
            InsertLens.Cons(@",");

        var source = "Jane Doe: ";
        var expectedTarget = "Jane Doe,";

        var target = lens.PutRight(source, Option.None<string>());

        Assert.Equal(expectedTarget, target);

    }

    [Fact]
    public void RoundTrip_OnIterateLens()
    { 
        var source = "John;Paul;Alice;George;Dicky;Stuart;Pete";
        var target = new[] { "John", "Paul", "Alice", "George", "Dicky", "Stuart", "Pete" };
        var updatedTarget = new[] { "John", "George", "Alice", "Dicky", "Pete", "Gregory" };
        var updatedSource = "John;George;Alice;Dicky;Pete;Gregory";

        var lens = Combinators.Iterate(";", IdentityLens.Cons(@"\w+"));

        var result = lens.CreateRight(source)
            .Bind(right => lens.PutLeft(updatedTarget, Option.Some(source)));

        Assert.True(result);
        Assert.Equal(updatedSource, result.Data);
    }
}
