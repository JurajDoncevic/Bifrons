using Bifrons.Lenses.Symmetric.Relational.Model;

namespace Bifrons.Lenses.Symmetric.Relational.Columns;

public sealed class RenameLens : SymmetricColumnLens
{
    private readonly string _sourceColumnName;
    private readonly string _targetColumnName;

    public string SourceColumnName => _sourceColumnName;
    public override string TargetColumnName => _targetColumnName;

    private RenameLens(string sourceColumnName, string targetColumnName)
    {
        _sourceColumnName = sourceColumnName;
        _targetColumnName = targetColumnName;
    }

    public override Func<Column, Option<Column>, Result<Column>> PutLeft =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(Column.Cons(_sourceColumnName, target.DataType)),
                () => CreateLeft(updatedSource)
            );

    public override Func<Column, Option<Column>, Result<Column>> PutRight =>
        (updatedSource, originalTarget) =>
            originalTarget.Match(
                target => Result.Success(Column.Cons(_targetColumnName, target.DataType)),
                () => CreateRight(updatedSource)
            );

    public override Func<Column, Result<Column>> CreateRight =>
        source => Result.Success(Column.Cons(_targetColumnName, source.DataType));

    public override Func<Column, Result<Column>> CreateLeft =>
        source => Result.Success(Column.Cons(_sourceColumnName, source.DataType));

    public static RenameLens Cons(string sourceColumnName, string targetColumnName)
        => new(sourceColumnName, targetColumnName);


}