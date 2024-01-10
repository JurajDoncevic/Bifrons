namespace Bifrons.Lenses.Symmetric.Integers;

public sealed class SubLens : SymmetricIntegerLens
{
    private readonly int _subValue;

    private SubLens(int subValue)
    {
        _subValue = subValue;
    }

    public override Func<int, Option<int>, Result<int>> PutLeft => 
        (updatedSource, _) => Results.OnSuccess(updatedSource + _subValue);

    public override Func<int, Option<int>, Result<int>> PutRight => 
        (updatedSource, _) => Results.OnSuccess(updatedSource - _subValue);

    public override Func<int, Result<int>> CreateRight => 
        source => Results.OnSuccess(source - _subValue);

    public override Func<int, Result<int>> CreateLeft => 
        source => Results.OnSuccess(source + _subValue);

    public static SubLens Cons(int subValue) => new(subValue);
}
