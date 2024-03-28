namespace Bifrons.Lenses.Strings;

public class MergeRight : ISymmetricLens<Either<string, string>, string>
{
    private readonly OrLens _orLens;
    private readonly SymmetricStringLens _stringLens;

    private MergeRight(OrLens orLens, SymmetricStringLens stringLens)
    {
        _orLens = orLens;
        _stringLens = stringLens;
    }

    public Func<string, Option<Either<string, string>>, Result<Either<string, string>>> PutLeft =>
        (_, _) => Result.Exception<Either<string, string>>(new NotImplementedException());

    public Func<Either<string, string>, Option<string>, Result<string>> PutRight =>
        (updatedSource, originalTarget) => updatedSource.Match(
            left => _stringLens.PutRight(left, originalTarget),
            right => _stringLens.PutRight(right, originalTarget)
            );

    public Func<Either<string, string>, Result<string>> CreateRight =>
        source => source.Match(
            left => _stringLens.CreateRight(left),
            right => _stringLens.CreateRight(right)
            );

    public Func<string, Result<Either<string, string>>> CreateLeft =>
        _ => Result.Exception<Either<string, string>>(new NotImplementedException());

    public static MergeRight Cons(OrLens orLens, SymmetricStringLens stringLens)
        => new MergeRight(orLens, stringLens);
}

