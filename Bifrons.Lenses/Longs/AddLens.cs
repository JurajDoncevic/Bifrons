namespace Bifrons.Lenses.Longs;

/// <summary>
/// Describes a lens that adds a constant value to a long.
/// add(value) : long <=> long
/// </summary>
public class AddLens : SymmetricLongLens
{
    private readonly long _addValue;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="addValue">The value to add to the source</param>
    protected AddLens(long addValue)
    {
        _addValue = addValue;
    }

    public override Func<long, Option<long>, Result<long>> PutLeft => 
        (updatedSource, _) => Result.Success(updatedSource - _addValue);

    public override Func<long, Option<long>, Result<long>> PutRight => 
        (updatedSource, _) => Result.Success(updatedSource + _addValue);

    public override Func<long, Result<long>> CreateRight => 
        source => Result.Success(source + _addValue);

    public override Func<long, Result<long>> CreateLeft => 
        source => Result.Success(source - _addValue);

    /// <summary>
    /// Constructs an addition lens
    /// </summary>
    /// <param name="addValue">Addition value</param>
    public static AddLens Cons(long addValue) => new(addValue);
}
