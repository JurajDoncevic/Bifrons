using Bifrons.Lenses.RelationalData.Columns;
using Bifrons.Lenses.RelationalData.Model;

namespace Bifrons.Lenses.RelationalData.Tables;

public sealed class InsertLens: SymmetricTableDataLens
{
    private InsertLens(Relational.Tables.InsertLens tableLens) 
        : base(tableLens, Option.None<IEnumerable<ISymmetricColumnDataLens>>())
    {

    }

    public override Func<TableData, Option<TableData>, Result<TableData>> PutLeft => 
        (updatedSource, originalTarget) => originalTarget.Match(
            original => _tableLens.PutLeft(updatedSource.Table, original.Table)
                                  .Bind(table => TableData.Cons(table, original.RowData)),
            () => CreateLeft(updatedSource)
            );

    public override Func<TableData, Option<TableData>, Result<TableData>> PutRight => 
        (updatedSource, originalTarget) => originalTarget.Match(
            original => _tableLens.PutRight(updatedSource.Table, original.Table)
                                  .Bind(table => TableData.Cons(table, original.RowData)),
            () => CreateRight(updatedSource)
            );

    public override Func<TableData, Result<TableData>> CreateRight =>
        source => _tableLens.CreateRight(source.Table)
            .Bind(table => TableData.Cons(table, []));

    public override Func<TableData, Result<TableData>> CreateLeft => 
        source => _tableLens.CreateLeft(source.Table)
            .Bind(table => TableData.Cons(table, []));

    public static InsertLens Cons(Relational.Tables.InsertLens tableLens) 
        => new(tableLens);
}
