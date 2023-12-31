﻿namespace Bifrons.Lenses.Symmetric.Strings;

public class MergeRight : BaseSymmetricLens<Either<string, string>, string>
{
    private readonly OrLens _orLens;
    private readonly SymmetricStringLens _stringLens;

    private MergeRight(OrLens orLens, SymmetricStringLens stringLens)
    {
        _orLens = orLens;
        _stringLens = stringLens;
    }

    public override Func<string, Option<Either<string, string>>, Result<Either<string, string>>> PutLeft =>
        (_, _) => Results.OnException<Either<string, string>>(new NotImplementedException());

    public override Func<Either<string, string>, Option<string>, Result<string>> PutRight =>
        (either, originalSource) => either.Match(
            left => _stringLens.PutRight(left, originalSource),
            right => _stringLens.PutRight(right, originalSource)
            );

    public override Func<Either<string, string>, Result<string>> CreateRight =>
        either => either.Match(
            left => _stringLens.CreateRight(left),
            right => _stringLens.CreateRight(right)
            );

    public override Func<string, Result<Either<string, string>>> CreateLeft =>
        _ => Results.OnException<Either<string, string>>(new NotImplementedException());

    public static MergeRight Cons(OrLens orLens, SymmetricStringLens stringLens)
        => new MergeRight(orLens, stringLens);
}

