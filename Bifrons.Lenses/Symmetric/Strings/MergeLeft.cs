namespace Bifrons.Lenses.Symmetric.Strings;

public class MergeLeft : BaseSymmetricLens<string, Either<string, string>>
{
    private readonly OrLens _orLens;
    private readonly SymmetricStringLens _stringLens;

    private MergeLeft(OrLens orLens, SymmetricStringLens stringLens)
    {
        _orLens = orLens;
        _stringLens = stringLens;
    }

    public override Func<Either<string, string>, Option<string>, Result<string>> PutLeft =>
        (either, originalSource) => either.Match(
            left => _stringLens.PutLeft(left, originalSource),
            right => _stringLens.PutLeft(right, originalSource)
            );

    public override Func<string, Option<Either<string, string>>, Result<Either<string, string>>> PutRight =>
        (_, _) => Results.OnException<Either<string, string>>(new NotImplementedException());

    public override Func<string, Result<Either<string, string>>> CreateRight =>
        _ => Results.OnException<Either<string, string>>(new NotImplementedException());

    public override Func<Either<string, string>, Result<string>> CreateLeft =>
        either => either.Match(
            left => _stringLens.CreateLeft(left),
            right => _stringLens.CreateLeft(right)
            );

    public static MergeLeft Cons(OrLens orLens, SymmetricStringLens stringLens)
        => new(orLens, stringLens);
}

