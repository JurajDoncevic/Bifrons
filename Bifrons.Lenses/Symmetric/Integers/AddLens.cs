namespace Bifrons.Lenses.Symmetric.Integers;

public class AddLens : SymmetricIntegerLens
{
    private readonly int _addValue;

    protected AddLens(int addValue)
    {
        _addValue = addValue;
    }

    public override Func<int, Option<int>, Result<int>> PutLeft => 
        (updatedSource, _) => Results.OnSuccess(updatedSource - _addValue);

    public override Func<int, Option<int>, Result<int>> PutRight => 
        (updatedSource, _) => Results.OnSuccess(updatedSource + _addValue);

    public override Func<int, Result<int>> CreateRight => 
        source => Results.OnSuccess(source + _addValue);

    public override Func<int, Result<int>> CreateLeft => 
        source => Results.OnSuccess(source - _addValue);

    public static AddLens Cons(int addValue) => new(addValue);
}
