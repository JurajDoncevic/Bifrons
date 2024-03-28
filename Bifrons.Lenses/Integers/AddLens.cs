namespace Bifrons.Lenses.Integers;

/// <summary>
/// Describes a lens that adds a constant value to an integer.
/// add(value) : int <=> int
/// </summary>
public class AddLens : SymmetricIntegerLens
{
    private readonly int _addValue;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="addValue">The value to add to the source</param>
    protected AddLens(int addValue)
    {
        _addValue = addValue;
    }

    public override Func<int, Option<int>, Result<int>> PutLeft => 
        (updatedSource, _) => Result.Success(updatedSource - _addValue);

    public override Func<int, Option<int>, Result<int>> PutRight => 
        (updatedSource, _) => Result.Success(updatedSource + _addValue);

    public override Func<int, Result<int>> CreateRight => 
        source => Result.Success(source + _addValue);

    public override Func<int, Result<int>> CreateLeft => 
        source => Result.Success(source - _addValue);

    /// <summary>
    /// Constructs an addition lens
    /// </summary>
    /// <param name="addValue">Addition value</param>
    public static AddLens Cons(int addValue) => new(addValue);
}
