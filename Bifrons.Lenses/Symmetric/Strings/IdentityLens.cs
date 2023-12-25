
namespace Bifrons.Lenses;

public class IdentityLens : BaseSymmetricLens<string, string>
{
    private IdentityLens() { }

    public override Func<string, Option<string>, Result<string>> PutLeft =>
        (right, _) => Results.OnSuccess(right);

    public override Func<string, Option<string>, Result<string>> PutRight =>
        (left, _) => Results.OnSuccess(left);

    public override Func<string, Result<string>> CreateRight =>
        left => Results.OnSuccess(left);

    public override Func<string, Result<string>> CreateLeft =>
        right => Results.OnSuccess(right);

    public static IdentityLens Cons() => new();
}
