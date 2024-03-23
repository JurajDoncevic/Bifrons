namespace Bifrons.Lenses.Symmetric.Strings;

public class MergeLeft : ISimpleSymmetricLens<string, Either<string, string>>
{
    private readonly OrLens _orLens;
    private readonly SymmetricStringLens _stringLens;

    private MergeLeft(OrLens orLens, SymmetricStringLens stringLens)
    {
        _orLens = orLens;
        _stringLens = stringLens;
    }

    public Func<Either<string, string>, Option<string>, Result<string>> PutLeft =>
        (updatedSource, originalTarget) => updatedSource.Match(
            left => _stringLens.PutLeft(left, originalTarget),
            right => _stringLens.PutLeft(right, originalTarget)
            );

    public Func<string, Option<Either<string, string>>, Result<Either<string, string>>> PutRight =>
        (_, _) => Result.Exception<Either<string, string>>(new NotImplementedException());

    public Func<string, Result<Either<string, string>>> CreateRight =>
        _ => Result.Exception<Either<string, string>>(new NotImplementedException());

    public Func<Either<string, string>, Result<string>> CreateLeft =>
        source => source.Match(
            left => _stringLens.CreateLeft(left),
            right => _stringLens.CreateLeft(right)
            );

    public static MergeLeft Cons(OrLens orLens, SymmetricStringLens stringLens)
        => new(orLens, stringLens);
}

