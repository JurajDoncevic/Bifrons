namespace Bifrons.Lenses.Symmetric.Integers;

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
        (updatedSource, _) => Results.OnSuccess(updatedSource - _addValue);

    public override Func<int, Option<int>, Result<int>> PutRight => 
        (updatedSource, _) => Results.OnSuccess(updatedSource + _addValue);

    public override Func<int, Result<int>> CreateRight => 
        source => Results.OnSuccess(source + _addValue);

    public override Func<int, Result<int>> CreateLeft => 
        source => Results.OnSuccess(source - _addValue);

    /// <summary>
    /// Constructs an addition lens
    /// </summary>
    /// <param name="addValue">Addition value</param>
    public static AddLens Cons(int addValue) => new(addValue);
}
